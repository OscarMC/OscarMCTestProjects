using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using uboot.ExtendedImmediateWindow.Implementation.Helpers;
using EnvDTE;

namespace uboot.ExtendedImmediateWindow.Implementation
{
    public class Injector
    {
        internal Injector(Debugger debugger, string source, List<Assembly> refAssemblies, List<LocalMember> localMembers)
        {
            Debugger = debugger;
            SourceCode = source;
            ReferencedAssemblies = refAssemblies;
            LocalMembers = localMembers;
        }

        public Debugger Debugger { get; set; }
        public string SourceCode { get; set; }
        public List<Assembly> ReferencedAssemblies { get; set; }
        public List<LocalMember> LocalMembers { get; set; }
        public Compiler Compiler { get; set; }

        public void Execute()
        {
            //Den Injector-Code in eine dynamisch kompilierte Assembly kompilieren
            bool success = TryCompileSourceCode();

            if (!success)
                return;

            //Die dynamisch kompilierte Assembly in den Debuggee laden
            LoadInjectorIntoDebuggee();

            //Locals in den Injector kopieren
            CopyOriginalLocalsToInjector();
                        
            //Methode ausführen            
            Expression finalExp = Inject("___method.Invoke(___injector, new object[] { });");

            //Modifizierte Locals zurück in den Debuggee kopieren
            CopyModifiedLocalsFromInjector();

            //Ergebnis ausgeben
            var resArgs = new ResultEventArgs()
            {
                Success = true,
                CompilingFailed = false,
                ResultExpression = finalExp
            };
            if (Results != null)
                Results.Invoke(this, resArgs);
                
        }

        private void CopyModifiedLocalsFromInjector()
        {
            //Da 'ref' aus irgendeinem Grund nicht funktioniert, die Werte holen.            
            foreach (var field in Compiler.LocalFields)
            {
                Inject(string.Format(
                    "{0} = ({1})___injectorType.GetField(\"{0}\").GetValue(___injector);",
                    field.CompilableName,
                    field.CompilableTypeName));
            }
        }

        private void CopyOriginalLocalsToInjector()
        {
            //'this' kopieren           
            Inject("___injectorType.GetField(\"m___this\").SetValue(___injector, this);");

            //Da 'ref' aus irgendeinem Grund nicht funktioniert, die Methoden-Locals kopieren.            
            foreach (var field in Compiler.LocalFields)
                Inject(string.Format("___injectorType.GetField(\"{0}\").SetValue(___injector, {0});", field.CompilableName));
        }

        private void LoadInjectorIntoDebuggee()
        {
            Inject("System.Reflection.Assembly ___assembly;");
            Inject("Type ___injectorType;");
            Inject("object ___injector;");
            Inject("System.Reflection.MethodInfo ___method;");
            Inject("___assembly = System.Reflection.Assembly.LoadFile(@\"" + Compiler.OutputAssemblyPath + "\");");
            Inject("___injectorType = ___assembly.GetType(\"MethodInjector\");");
            Inject("___injector = ___injectorType.GetConstructor(new Type[] { }).Invoke(new object[] { });");
            Inject("___method = ___injectorType.GetMethod(\"InjectionMethod\");");
        }

        private Expression Inject(string statement)
        {                        
            var result = Debugger.GetExpression(statement, true, 200);
            return result;
        }

        private bool TryCompileSourceCode()
        {
            var compiler = new Compiler(SourceCode, ReferencedAssemblies, LocalMembers, LocalsMode.PrivateClassFields);
            compiler.Compile();

            if (compiler.Results.Errors.HasErrors)
            {
                var resArgs = new ResultEventArgs()
                {
                    Success = false,
                    CompilingFailed = true,
                    ErrorMessage = compiler.ErrorText
                };
                if (Results != null)
                    Results.Invoke(this, resArgs);
                return false;
            }
            else
            {
                Compiler = compiler;
                return true;
            }
        }

        public class ResultEventArgs : EventArgs
        {
            public bool Success { get; set; }
            public bool CompilingFailed { get; set; }
            public string ErrorMessage { get; set; }
            public Expression ResultExpression { get; set; }
        }
        public event EventHandler<ResultEventArgs> Results;

    }
}
