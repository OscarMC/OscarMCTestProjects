using System.Security.Permissions;
using System.Windows.Forms;
using System;
using uboot.ExtendedImmediateWindow.Implementation;
using EnvDTE;

namespace uboot.ExtendedImmediateWindow
{
    /// <summary>
    /// Summary description for MyControl.
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        public ToolWindowControl()
        {
            this.Visible = false;
            InitializeComponent();
            DoubleBuffered = true;
            intellisenseTextBox1.Text =
                "string msg = "+Environment.NewLine+
                "\tstring.Format("+Environment.NewLine+
                "\t\"Hello World from inside {0}!\", "+Environment.NewLine+
                "\tthis);" + Environment.NewLine +
                "System.Console.WriteLine(msg);" + Environment.NewLine +
                Environment.NewLine +
                "/* You can see the result of this intruding method if you turn on the visual studio output window. (menue: View -> Output) */";
            this.Visible = true;
        }

        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
            // If we're the top-level form or control, we need to do the mnemonic handling
            if (charCode != ' ' && ProcessMnemonic(charCode))
            {
                return true;
            }
            return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// Enable the IME status handling for this control.
        /// </summary>
        protected override bool CanEnableIme
        {
            get
            {
                return true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show(this,
                            string.Format(System.Globalization.CultureInfo.CurrentUICulture, "We are inside {0}.button1_Click()", this.ToString()),
                            "Extended Immediate Tool Window");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new SplashScreen().Show();
        }

        private void resultControl1_ParentChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = (resultControl1.Parent != splitContainer1.Panel2);
        }

        internal class RunIntrudingMethodEventArgs : EventArgs
        {
            public string Source { get; set; }
        }
        internal event EventHandler<RunIntrudingMethodEventArgs> RunIntrudingMethod;

        internal class QueryContextTypeEventArgs : EventArgs
        {
            public Type ContextType { get; set; }
        }
        internal event EventHandler<QueryContextTypeEventArgs> QueryContextType;

        private void btRun_Click(object sender, EventArgs e)
        {
            if (RunIntrudingMethod != null)
                RunIntrudingMethod.Invoke(this, new RunIntrudingMethodEventArgs() { Source = intellisenseTextBox1.Text });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (QueryContextType != null)
            {
                var args = new QueryContextTypeEventArgs();
                QueryContextType.Invoke(this, args);
                if (args.ContextType != null)
                    intellisenseTextBox1.ContextType = args.ContextType;
            }
        }

        public void SetContextType(Type contextType)
        {
            intellisenseTextBox1.ContextType = contextType;
            IsDebuggerConnected = true;
            btRun.Enabled = true;
            btRun.Text = "Run Intruding Method";
        }

        public bool IsDebuggerConnected { get; set; }

        internal event EventHandler Connect;

        private void connectToolStripButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (Connect != null)
                Connect.Invoke(this, EventArgs.Empty);
            this.Cursor = Cursors.Arrow;
        }

        private void intellisenseTextBox1_Load(object sender, EventArgs e)
        {

        }

        private void intellisenseTextBox1_SourceCodeChanged(object sender, EventArgs e)
        {
            if (!IsDebuggerConnected && !IsNotCottectedMessageShown)
            {
                string msg = "You are not yet connected to a debugger, intellisense environment may be wrong. Please connect before to a running debugger before executing the intruding method.";
                toolTip1.Show(msg, intellisenseTextBox1, 20000);
                IsNotCottectedMessageShown = true;
            }
        }

        private bool IsNotCottectedMessageShown { get; set; }

        private void resultControl1_Load(object sender, EventArgs e)
        {

        }


        internal void TellExecutionFailed(string message)
        {
            resultControl1.Enabled = false;
            resultControl1.TellSingleMessage(message);
        }

        internal void DisplayResultExpression(Expression expression)
        {
            resultControl1.Enabled = true;
            resultControl1.DisplayExpression(expression);
        }
    }
}
