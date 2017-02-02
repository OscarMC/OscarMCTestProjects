using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using EnvDTE80;
using EnvDTE;
using uboot.ExtendedImmediateWindow.Implementation.Extensions;

namespace uboot.ExtendedImmediateWindow.Implementation.Helpers
{
    public class ReferenceResolver
    {     

        public ReferenceResolver(string rootAssemblyPath)
        {
            if (m_loadedAssemblies.ContainsKey(rootAssemblyPath))
                RootAssembly = m_loadedAssemblies[rootAssemblyPath];
            else
            {
                string path = TempFileNameService.GetNewFileName(Path.GetExtension(rootAssemblyPath));
                File.Copy(rootAssemblyPath, path, true);
                RootAssembly = Assembly.LoadFile(path);
                m_loadedAssemblies.Add(rootAssemblyPath, RootAssembly);
            }
        }

        public ReferenceResolver(Assembly rootAssembly)
        {
            RootAssembly = rootAssembly;
        }

        private static Dictionary<string, Assembly> m_loadedAssemblies = new Dictionary<string, Assembly>();

        public Assembly RootAssembly { get; set; }

        public DTE2 VsApplication { get; set; }

        private static Dictionary<Assembly, List<Assembly>> m_referenceLists = new Dictionary<Assembly, List<Assembly>>();

        public List<Assembly> ReferencedAssemblies
        {
            get
            {
                if (!m_referenceLists.ContainsKey(RootAssembly))
                {
                    List<Assembly> allAssemblies = new List<Assembly>();

                    foreach (var process in VsApplication.Debugger.DebuggedProcesses.Cast<Process>())
                    {
                        try
                        {
                            int processId = process.ProcessID;
                            var dotNetProcess = System.Diagnostics.Process.GetProcessById(processId);

                            int exceptionCount = int.MaxValue - 1;
                            int lastExceptionCount = int.MaxValue;
                            while (exceptionCount < lastExceptionCount)
                            {
                                lastExceptionCount = exceptionCount;
                                exceptionCount = 0;
                                foreach (var module in dotNetProcess.Modules.Cast<System.Diagnostics.ProcessModule>())
                                {
                                    try
                                    {
                                        string fileName = module.FileName;
                                        var assembly = Assembly.LoadFile(fileName);
                                        allAssemblies.Add(assembly);
                                    }
                                    catch (Exception exc) 
                                    {
                                        if (exc is BadImageFormatException)
                                            System.Diagnostics.Debug.WriteLine(" -> " + module.ModuleName + " is propably a unmanaged file");
                                        exceptionCount++; 
                                    }
                                }
                            }
                        }
                        catch (Exception harglgargl)
                        {
                            MessageBox.Show(harglgargl.ToString());
                        }
                    }

                    List<string> paths = new List<string>();
                    allAssemblies.ForEach(a => paths.Add(Path.GetDirectoryName(a.Location)));

                    List<Assembly> assemblies = new List<Assembly>();
                    foreach (var assembly in allAssemblies)
                    {
                        var subAssemblies =
                            RootAssembly
                            .GetRecursive(
                                new List<Assembly>(),
                                (a, list) => a.GetReferencedAssemblies().Select(an => TryToLoadAssembly(paths, an)).Where(ass => !list.Contains(ass))
                            );
                        assemblies.AddRange(subAssemblies);
                    }
                    assemblies.Add(RootAssembly);
                    var referenceList = assemblies.Where(a => a != null).Distinct().ToList();
                    m_referenceLists.Add(RootAssembly, referenceList);
                }
                return m_referenceLists[RootAssembly];
            }
        }

        private Assembly TryToLoadAssembly(List<string> paths, AssemblyName assemblyName)
        {
            Assembly ass = null;
            try
            {
                ass = Assembly.Load(assemblyName);
            }
            catch
            {
                foreach (string path in paths)
                {
                    try
                    {
                        string file = Path.Combine(path, assemblyName.Name + ".dll");
                        ass = Assembly.LoadFile(file);
                        break;
                    }
                    catch {}
                }
            }
            return ass;
        }

    }
}
