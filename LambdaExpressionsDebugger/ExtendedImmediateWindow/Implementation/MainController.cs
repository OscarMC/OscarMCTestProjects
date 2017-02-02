using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Windows.Forms;
using uboot.ExtendedImmediateWindow.Implementation.Extensions;
using System.Reflection;
using uboot.ExtendedImmediateWindow.Implementation.Helpers;

namespace uboot.ExtendedImmediateWindow.Implementation
{
    internal class MainController
    {
        public MainController(VsToolWindow toolWindow, DTE2 dte2, Package package)
        {
            Package = package;
            DTE = dte2;
            ViewController = new ViewController(toolWindow);
            ViewController.RunIntrudingMethod += new EventHandler<ToolWindowControl.RunIntrudingMethodEventArgs>(ViewController_RunIntrudingMethod);
            ViewController.QueryContextType += new EventHandler<ToolWindowControl.QueryContextTypeEventArgs>(ViewController_QueryContextType);
            ViewController.QueryConnect += new EventHandler(ViewController_QueryConnect);
            IsConnected = false;
            StartContinousContextUpdate();
        }

        void ViewController_QueryConnect(object sender, EventArgs e)
        {
            try
            {                
                UpdateContextTypeIfChanged();
                IsDebuggerConnected = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Konnte nicht verbinden: "+exc.Message+" "+exc.StackTrace);
            }
        }

        void TryConnectToDebugger()
        {
            try
            {
                UpdateContextTypeIfChanged();
                IsDebuggerConnected = true;
            }
            catch (Exception exc)
            {
                Log("Konnte nicht verbinden: " + exc.Message + " " + exc.StackTrace);
            }
        }

        public bool IsDebuggerConnected {get;set;}

        public bool ContinousContextUpdateIsRunning { get; set; }
        private void StartContinousContextUpdate()
        {
            Action updateAction = () =>
                {
                    while (ContinousContextUpdateIsRunning)
                    {
                        TryConnectToDebugger();
                        System.Threading.Thread.Sleep(10000);
                    }
                };
            ContinousContextUpdateIsRunning = true;
            updateAction.BeginInvoke(null, null);            
        }

        private void UpdateContextTypeIfChanged()
        {
            if (IsConnected && DTE.Debugger.CurrentMode == dbgDebugMode.dbgBreakMode)
            {                
                string currentFunction = DTE.Debugger.CurrentStackFrame.FunctionName;

                Log("ExtendedImmediateAddInn - Connect");
                Log("Current Func: "+currentFunction);

                if (currentFunction != LastStackFunction)
                {
                    Log("-> not cached already, have to analyze " + currentFunction);                    
                    CurrentType = DTE.Debugger.GetCurrentType();
                    Log("CurrentType: " + CurrentType.ToString());
                    var context = GetContextType();
                    Log("ContextType (compiled in background): " + context.ToString());
                    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);                    
                    ViewController.SetContextType(context);
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                    LastStackFunction = currentFunction;
                    Log("Done.");                    
                }
            }
        }

        private void Log(string txt)
        {
            System.Diagnostics.Debug.WriteLine(txt);
        }

        public string LastStackFunction { get; set; }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == CurrentType.Assembly.FullName)
                return CurrentType.Assembly;
            else
                return null;            
        }

        void ViewController_QueryContextType(object sender, ToolWindowControl.QueryContextTypeEventArgs e)
        {
            if (DTE.Debugger.CurrentMode != dbgDebugMode.dbgBreakMode)
                return;

            e.ContextType = GetContextType();
        }

        private Type GetContextType()
        {
            CurrentType = DTE.Debugger.GetCurrentType();

            var resolver = DTE.Debugger.GetResolver();
            resolver.VsApplication = DTE;
            resolver.ReferencedAssemblies.ToList();

            Compiler compiler = new Compiler("", resolver.ReferencedAssemblies, LocalReachableMembers.ToList(), LocalsMode.PrivateClassFields);
            compiler.Compile();

            return compiler.InjectorType;
        }

        void ViewController_RunIntrudingMethod(object sender, ToolWindowControl.RunIntrudingMethodEventArgs e)
        {
            if (DTE.Debugger.CurrentMode != dbgDebugMode.dbgBreakMode)
            {
                MessageBox.Show("You have to be inside a debugging session to inject intruding methods", "Not Possible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Execute(e.Source);

        }

        private void Execute(string source)
        {
            var refAssemblies = DTE.Debugger.GetResolver().ReferencedAssemblies;
            var localMembers = LocalReachableMembers.ToList();

            Injector injector = new Injector(DTE.Debugger, source, refAssemblies, localMembers);
            injector.Results += new EventHandler<Injector.ResultEventArgs>(injector_Results);
            injector.Execute();            
            
        }

        void injector_Results(object sender, Injector.ResultEventArgs e)
        {
            if (e.CompilingFailed)
                this.ViewController.TellExecutionFailed(e.ErrorMessage);
            else
                this.ViewController.DisplayResultExpression(e.ResultExpression);
        }

        public IList<LocalMember> LocalReachableMembers
        {
            get
            {
                var localFields =
                    from e in DTE.Debugger.CurrentStackFrame.Locals.Cast<Expression>()
                    where !e.Name.StartsWith("___")
                    select new LocalMember() { Name = e.Name, TypeName = e.Type, Member = MemberType.Field };
                var publicInstanceProperties =
                    from e in CurrentType.GetProperties()
                    select new LocalMember() { Name = e.Name, Type = e.PropertyType, TypeName = e.PropertyType.FullName, Member = MemberType.Property, HasGetAccessor = e.CanRead, HasSetAccessor = e.CanWrite };
                var publicStaticProperties =
                    from e in CurrentType.GetProperties(BindingFlags.Static)
                    select new LocalMember() { Name = e.Name, Type = e.PropertyType, TypeName = e.PropertyType.FullName, Member = MemberType.Property, HasGetAccessor = e.CanRead, HasSetAccessor = e.CanWrite };
                var nonPublicInstanceProperties =
                    from e in CurrentType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                    select new LocalMember() { Name = e.Name, Type = e.PropertyType, TypeName = e.PropertyType.FullName, Member = MemberType.Property, HasGetAccessor = e.CanRead, HasSetAccessor = e.CanWrite };
                var nonPublicStaticProperties =
                    from e in CurrentType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic)
                    select new LocalMember() { Name = e.Name, Type = e.PropertyType, TypeName = e.PropertyType.FullName, Member = MemberType.Property, HasGetAccessor = e.CanRead, HasSetAccessor = e.CanWrite };
                var reachableMembers =
                    localFields
                    .Union(publicInstanceProperties)
                    .Union(publicStaticProperties)
                    .Union(nonPublicInstanceProperties)
                    .Union(nonPublicStaticProperties)
                    .OrderBy(a => a.Name);
                List<LocalMember> result = null;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        result = reachableMembers.ToList();
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                }
                return result;
            }
        }

        public Type CurrentType { get; set; }

        public Package Package { get; set; }

        public DTE2 DTE { get; set; }

        public ViewController ViewController { get; set; }

        private bool IsConnected { get; set; }

        public void Connect()
        {
            if (IsConnected) return;
            ViewController.Connect();
            IsConnected = true;
        }

    }
}
