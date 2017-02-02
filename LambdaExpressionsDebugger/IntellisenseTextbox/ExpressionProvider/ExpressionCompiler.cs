using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CSharp;

namespace IntellisenseTextbox
{
    class ExpressionCompiler
    {
        public ExpressionCompiler(ContextProvider context)
        {            
            m_context = context;
        }

        private ContextProvider m_context;
        public CompilerResults Results { get; set; }

        public const string GeneratedClassName = "DynamicCompileClass";
        public const string GeneratedMethodName = "GetExpression";

        public void Compile(string methodSourceCode)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var assemblyNames = (from a in m_context.Assemblies
                                 select a.Location).ToList();
            if (!assemblyNames.Any(an => an.ToLower().Contains("system.core.dll")))
                assemblyNames.Add("System.Core.dll");
            var parameters = new CompilerParameters(assemblyNames.ToArray());
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = false;

            string usings = GetUsingStatements();
            string properties = GetProperties();
            string fields = GetFields();

            string sourceCode =
                    usings +
                    @"public class " + GeneratedClassName + @" {

                        " + properties + @"
                        " + fields + @"

                        public Expression<Action> " + GeneratedMethodName + @"() 
                        {
                            Expression<Action> expression = () => 
                            {
                                " + methodSourceCode + @"
                            };
                            return expression;
                        }
                    }";
            Results = csc.CompileAssemblyFromSource(parameters, sourceCode);                        
        }

        private string GetFields()
        {
            return string.Concat((from i in m_context.Items
                                  where i.SymbolType == SymbolType.Field ||
                                    i.SymbolType == SymbolType.Constant ||
                                    i.SymbolType == SymbolType.Enum ||
                                    i.SymbolType == SymbolType.EnumItem
                                  select "private " + i.Type.Name + " " + i.Name + ";" + Environment.NewLine)
                                  .ToArray());
        }

        private string GetProperties()
        {
            return string.Concat((from i in m_context.Items
                                  where i.SymbolType == SymbolType.Property
                                  select "private " + i.Type.Name + " " + i.Name + " { get; set; };" + Environment.NewLine)
                                  .ToArray());
        }
               
        private string GetUsingStatements()
        {
            var usings = string.Concat((from i in m_context.Items
                                        where i.SymbolType == SymbolType.Namespace
                                        select "using " + i.Name + ";" + Environment.NewLine)
                                        .ToArray());
            if (!usings.Contains("using System.Linq;"))
                usings += "using System.Linq;" + Environment.NewLine;
            if (!usings.Contains("using System.Linq.Expressions;"))
                usings += "using System.Linq.Expressions;" + Environment.NewLine;
            return usings;            
        }

        
    }
}
