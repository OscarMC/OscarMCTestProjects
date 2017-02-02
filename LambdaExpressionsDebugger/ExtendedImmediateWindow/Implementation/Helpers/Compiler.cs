using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;


namespace uboot.ExtendedImmediateWindow.Implementation.Helpers
{
    public enum LocalsMode { PrivateClassFields, MethodParameters }

    public class Compiler
    {
        public Compiler(string methodBody, List<Assembly> referencedAssemblies, List<LocalMember> localReachableMembers, LocalsMode buildMode)
        {
            MethodBody = methodBody;
            AssemblyReferences = referencedAssemblies;
            if (!localReachableMembers.Any(l => l.Name == "m___this"))
                localReachableMembers.Add(new LocalMember() { Name = "m___this", Member = MemberType.Field, Type = typeof(object), TypeName = "System.Object" });
            Locals = localReachableMembers;
            LocalsCreationMode = buildMode;
        }

        private List<Assembly> AssemblyReferences { get; set; }

        private string MethodBody { get; set; }

        private List<LocalMember> Locals { get; set; }

        public Type InjectorType
        {
            get { return OutputAssembly.GetType("MethodInjector"); }
        }

        public Assembly OutputAssembly { get; set; }

        private string m_outputAssemblyPath;
        public string OutputAssemblyPath
        {
            get
            {
                if (m_outputAssemblyPath == null)
                    GetNewTempOutputPath();
                return m_outputAssemblyPath;
            }
            set { m_outputAssemblyPath = value; }
        }

        public bool HasErrors
        {
            get { return (Results == null || Results.Errors.HasErrors); }
        }

        private LocalsMode LocalsCreationMode { get; set; }

        public CompilerResults Results { get; set; }

        public Assembly CompiledAssembly { get { return Results.CompiledAssembly; } }

        public string ErrorText
        {
            get
            {
                string errors = string.Empty;
                Results.Errors.Cast<CompilerError>().ToList().ForEach(error => errors += error.ErrorText + Environment.NewLine);
                return errors;
            }
        }

        public string ErrorTextDetailed
        {
            get
            {
                string errors = string.Empty;
                Results.Errors.Cast<CompilerError>().ToList().ForEach(error => errors += error.ToString() + Environment.NewLine);
                return errors;
            }
        }

        private string[] AssemblyFileNames
        {
            get
            {
                return AssemblyReferences.Select(a => a.Location).ToArray();
            }
        }

        #region Properties und Fields

        private List<string> m_properties;
        private List<string> PropertyList
        {
            get
            {
                if (m_properties == null)
                {
                    m_properties = new List<string>();
                    var props = Locals
                        .Where(l => l.Member == MemberType.Property)
                        .ToList();
                    foreach (var p in props)
                    {
                        m_properties.Add(
                            string.Format(
                                "                    public {0} {1} [ {2} {3} ]",
                                p.CompilableTypeName,
                                p.CompilableName,
                                (p.HasGetAccessor ? "get { return m___this.GetPropertyContent<" + p.CompilableTypeName + ">(\"" + p.CompilableName + "\"); }" : ""),
                                (p.HasSetAccessor ? "set { m___this.SetPropertyContent(value, \"" + p.CompilableName + "\"); }" : "")
                                ).Replace("[", "{").Replace("]", "}"));
                    }
                }
                return m_properties;
            }
        }

        private List<LocalMember> m_localFields;
        public List<LocalMember> LocalFields
        {
            get
            {
                if (m_localFields == null)
                {
                    m_localFields = (from l in Locals
                                     where l.CompilableName != "m___this" &&
                                        l.Member == MemberType.Field &&
                                        !l.Name.StartsWith("___") &&
                                        !l.Name.StartsWith("$")
                                     select l).ToList();
                }
                return m_localFields;
            }
        }

        private LocalMember ThisMember
        {
            get
            {
                return (from l in Locals
                        where l.CompilableName == "m___this"
                        select l).FirstOrDefault();
            }
        }

        public List<LocalMember> UnavailableLocalFields  //wegen private / internal accessibility der verwendeten typen
        {
            get
            {
                return Locals
                    .Where(l => l.Member == MemberType.Field && !l.Name.StartsWith("___"))
                    .ToList()
                    .FindAll(l => !LocalFields.Any(lf => lf.Name == l.Name))
                    .ToList();
            }
        }

        private List<string> m_fields;
        private List<string> FieldList
        {
            get
            {
                if (m_fields == null)
                    m_fields = GetFieldList(LocalsCreationMode);
                return m_fields;
            }
        }

        private List<string> GetFieldList(LocalsMode LocalsCreationMode)
        {
            if (LocalsCreationMode == LocalsMode.MethodParameters)
                return new List<string>();
            else
                return (from l in LocalFields
                        select string.Format("                    public {0} {1};",
                            l.CompilableTypeName,
                            l.CompilableName)).ToList();
        }

        #endregion

        #region Template-Contents

        private List<string> m_usedNamespaces;
        private List<string> UsedNamespaces
        {
            get
            {
                if (m_usedNamespaces == null)
                {
                    m_usedNamespaces = (from a in AssemblyReferences
                                        from t in a.GetTypes()
                                        where t.Namespace != null && t.Namespace.Trim().Length > 0
                                        select t.Namespace).Distinct().ToList();
                }
                return m_usedNamespaces;
            }
        }

        private StringBuilder m_usings;
        private string Usings
        {
            get
            {
                if (m_usings == null)
                {
                    m_usings = new StringBuilder();
                    UsedNamespaces.ForEach(u =>
                    {
                        if (!u.StartsWith("<"))
                        {
                            m_usings.Append("using ");
                            m_usings.Append(u);
                            m_usings.AppendLine(";");
                        }
                    });
                }
                return m_usings.ToString();
            }
        }

        private StringBuilder m_refParams;
        private string MethodParameters
        {
            get
            {
                if (m_refParams == null)
                {
                    m_refParams = new StringBuilder();
                    if (LocalsCreationMode == LocalsMode.MethodParameters)
                    {
                        var localFields = Locals.Where(
                            l =>
                                l.Member == MemberType.Field &&
                                l.CompilableName != "m___this" &&
                                !l.Name.StartsWith("___") &&
                                !l.Name.StartsWith("$")
                                ).ToList();
                        for (int i = 0; i < localFields.Count; i++)
                        {
                            m_refParams.Append("ref ");
                            m_refParams.Append(localFields[i].CompilableTypeName);
                            m_refParams.Append(" ");
                            m_refParams.Append(localFields[i].CompilableName);
                            if (i < localFields.Count - 1)
                                m_refParams.Append(", ");
                        };
                    }
                }
                return m_refParams.ToString();
            }
        }

        #endregion

        private void GetNewTempOutputPath()
        {
            m_outputAssemblyPath = TempFileNameService.GetNewFileName("dll");
        }

        private string GetAllLinesFromList(List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
                sb.AppendLine(s);
            return sb.ToString();
        }

        private CompilerResults CheckForUnreachableTypes()
        {
            string propertySection = GetAllLinesFromList(PropertyList);
            string fieldsSection = GetAllLinesFromList(GetFieldList(LocalsMode.PrivateClassFields));
            CompilerResults results = CompileInternal(propertySection, fieldsSection, "");

            //Falls Errors in der Propertydeklaration aufgetreten sind 
            //(accessibility von Types), versuchen, die Properties rauszuschmeissen
            if (results.Errors.HasErrors)
            {
                int propertyListStartLine = UsedNamespaces.Count + 3;
                int propertyListEndLine = propertyListStartLine + PropertyList.Count;
                int fieldListStartLine = propertyListEndLine + 1;
                int fieldListEndLine = fieldListStartLine + FieldList.Count;
                bool errorsOutsideLocalsListDetected = false;
                foreach (var e in results.Errors.Cast<CompilerError>())
                {
                    if (e.Line < propertyListStartLine || e.Line > fieldListEndLine)
                    {
                        errorsOutsideLocalsListDetected = true;
                        break;
                    }
                }

                if (!errorsOutsideLocalsListDetected)
                {
                    foreach (var e in results.Errors.Cast<CompilerError>())
                    {
                        int indexWithinPropertyList = e.Line - propertyListStartLine - 1;
                        int indexWithinFieldList = e.Line - fieldListStartLine - 1;
                        if (indexWithinPropertyList >= 0 &&
                            indexWithinPropertyList < PropertyList.Count)
                            PropertyList.RemoveAt(indexWithinPropertyList);
                        if (indexWithinFieldList >= 0 &&
                            indexWithinFieldList < FieldList.Count)
                            FieldList.RemoveAt(indexWithinFieldList);
                        propertySection = GetAllLinesFromList(PropertyList);
                        fieldsSection = GetAllLinesFromList(FieldList);
                        GetNewTempOutputPath();
                        results = CompileInternal(propertySection, fieldsSection, "");
                    }
                }
            }

            return results;
        }

        private CompilerResults CompileInternal(string propertySection, string fieldsSection, string methodParams)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            var parameters = new CompilerParameters(AssemblyFileNames, OutputAssemblyPath);
            parameters.GenerateExecutable = false;
            //parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = false;

            string sourceCode =
                    Usings + @"
                    public class MethodInjector {

                        " + propertySection + @"
                        " + fieldsSection + @"
                        public " + ThisMember.CompilableTypeName + @" m___this;

                        public object InjectionMethod(" + methodParams + @") 
                        {
                            " + MethodBody + @"
                            return null;
                        }

                    }

                    public static class ReflectionExtensions
                    {
                        public static T GetPropertyContent<T>(this object source, string propName)
                        {
                            PropertyInfo pi = GetProperty(source, propName);
                            return (T)pi.GetValue(source, null);
                        }

                        public static void SetPropertyContent(this object source, object value, string propName)
                        {
                            PropertyInfo pi = GetProperty(source, propName);
                            pi.SetValue(source, value, null);
                        }

                        private static PropertyInfo GetProperty(object source, string propName)
                        {
                            var bindingFlagVariants =
                                new[] { 
                                    (BindingFlags.Instance),
                                    (BindingFlags.Instance | BindingFlags.NonPublic),
                                    (BindingFlags.Static),
                                    (BindingFlags.Static | BindingFlags.NonPublic)
                                };
                            PropertyInfo pi = source.GetType().GetProperty(propName);
                            if (pi != null)
                                return pi;                            
                            foreach (var flags in bindingFlagVariants)
                            {
                                pi = source.GetType().GetProperty(propName, flags);
                                if (pi != null)
                                    break;
                            }
                            return pi;
                        }
                    }";
            sourceCode = sourceCode.Replace("{}", "");

            //using (StreamWriter sw = new StreamWriter(@"c:\temp\source.cs")) { sw.WriteLine(sourceCode); }

            var result = csc.CompileAssemblyFromSource(parameters, sourceCode);
            if (!result.Errors.HasErrors)
                OutputAssembly = result.CompiledAssembly;
            return result;
        }

        public bool Compile()
        {
            //Checken, ob es Typen gibtm die nicht erreichbar sind, wegen private oder internal, ggf. rausschmeissen
            CompilerResults results = CheckForUnreachableTypes();
            if (results.Errors.HasErrors)
            {
                Results = results;
                Reset();
                return false;
            }

            //Wenn Build erfolgreich und unserem Modus entsprach, als Rückgabe verwenden
            if (LocalsCreationMode == LocalsMode.PrivateClassFields)
            {
                OutputAssemblyPath = results.PathToAssembly;
                Results = results;
                Reset();
                return true;
            }

            //Wenn eingestellter Modus anders (Methodenparameter), nun nochmal mit anderen Sessings Builden
            GetNewTempOutputPath();
            string propertySection = GetAllLinesFromList(PropertyList);
            string fieldsSection = GetAllLinesFromList(FieldList);
            Results = CompileInternal(propertySection, fieldsSection, MethodParameters);
            Reset();
            if (Results.Errors.HasErrors)
                return false;
            else
                return true;
        }

        private void Reset()
        {
            m_localFields = null;
            m_properties = null;
            m_localFields = null;
            m_fields = null;
            m_refParams = null;
        }

    }
}
