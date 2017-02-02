using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System.Reflection;
using System.Windows.Forms;
using uboot.ExtendedImmediateWindow.Implementation.Helpers;
using uboot.ExtendedImmediateWindow.Implementation.Extensions;
using System.IO;

namespace uboot.ExtendedImmediateWindow.Implementation.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> GetRecursive<T>(this T rootElement, IList<T> alreadyFoundElements, Func<T, IList<T>, IEnumerable<T>> findSubElementsFunction)
        {
            var subElements = findSubElementsFunction(rootElement, alreadyFoundElements).ToList();
            if (subElements.Count == 0 || subElements == null) return new T[] { };
            alreadyFoundElements = alreadyFoundElements.Union(subElements).ToList();
            foreach (T subE in subElements)
                alreadyFoundElements = alreadyFoundElements.Union(subE.GetRecursive(alreadyFoundElements, findSubElementsFunction)).ToList();
            return alreadyFoundElements;
        }

        public static ReferenceResolver GetResolver(this Debugger debugger)
        {
            //Get all referenced assemblies
            string assemblyLocation = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                try
                {                    
                    assemblyLocation =
                        debugger.GetExpression("System.Reflection.Assembly.GetExecutingAssembly().Location;", false, 5000).Value;
                    assemblyLocation = assemblyLocation.Replace(@"\\", @"\").Replace("\"", "");
                    if (File.Exists(assemblyLocation))
                        break;  //es kann auch eine Message wie "Function evaluation timed out" zurückkommen
                }
                catch
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (!File.Exists(assemblyLocation) || string.IsNullOrEmpty(assemblyLocation))
                return null;            
            ReferenceResolver resolver = new ReferenceResolver(assemblyLocation);
            return resolver;
        }

        public static Type GetCurrentType(this Debugger debugger)
        {
            Type currentClassType = null;

            Expression typeFullnameExpr = debugger.GetExpression("this.GetType().FullName", false, 2000);
            string className = typeFullnameExpr.Value.Replace("\"", "");
            if (className.Contains("'this'")) {
                className = debugger.CurrentStackFrame.FunctionName;
                if (className.Contains("."))
                    className = className.Substring(0, className.LastIndexOf("."));
            }
            var resolver = debugger.GetResolver();
            resolver.VsApplication = debugger.DTE as DTE2;
            resolver.ReferencedAssemblies.ToList();
            currentClassType = (from t in resolver.RootAssembly.GetTypes()
                                     where LocalMember.RepairTypeName(t.FullName, t) == className
                                     select t).FirstOrDefault();                

            return currentClassType;
        }
    }
}
