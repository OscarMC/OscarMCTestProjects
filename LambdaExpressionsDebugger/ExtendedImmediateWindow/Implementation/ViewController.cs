using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace uboot.ExtendedImmediateWindow.Implementation
{
    internal class ViewController
    {
        public ViewController(VsToolWindow view)
        {
            VsToolWin = view;            
        }

        void View_RunIntrudingMethod(object sender, ToolWindowControl.RunIntrudingMethodEventArgs e)
        {
            if (RunIntrudingMethod != null)
                RunIntrudingMethod.Invoke(sender, e);
        }

        void View_QueryContextType(object sender, ToolWindowControl.QueryContextTypeEventArgs e)
        {
            //if (QueryContextType != null)
            //    QueryContextType.Invoke(sender, e);
        }

        public event EventHandler<ToolWindowControl.RunIntrudingMethodEventArgs> RunIntrudingMethod;
        public event EventHandler<ToolWindowControl.QueryContextTypeEventArgs> QueryContextType;

        private VsToolWindow VsToolWin { get; set; }
        private ToolWindowControl View
        {
            get
            {
                if (VsToolWin == null) return null;
                return VsToolWin.Control;
            }
        }

        public bool Connect()
        {
            if (View == null) return false;
            View.RunIntrudingMethod += new EventHandler<ToolWindowControl.RunIntrudingMethodEventArgs>(View_RunIntrudingMethod);
            View.QueryContextType += new EventHandler<ToolWindowControl.QueryContextTypeEventArgs>(View_QueryContextType);
            View.Connect += new EventHandler(View_Connect);
            return true;
        }

        internal event EventHandler QueryConnect;

        void View_Connect(object sender, EventArgs e)
        {
            if (QueryConnect != null)
                QueryConnect.Invoke(this, EventArgs.Empty);
        }

        public void SetContextType(Type contextType)
        {
            Action setContextTypeAction = () => View.SetContextType(contextType);
            View.Invoke(setContextTypeAction);
        }

        internal void TellExecutionFailed(string message)
        {
            View.TellExecutionFailed(message);
        }

        internal void DisplayResultExpression(Expression expression)
        {
            View.DisplayResultExpression(expression);
        }
    }
}
