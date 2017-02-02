using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntellisenseTextbox
{
    public partial class SyntaxTextBoxKeyHandler : Component
    {
        public SyntaxTextBoxKeyHandler()
        {
            InitializeComponent();
        }

        public SyntaxTextBoxKeyHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        private SyntaxTextBox m_syntaxTextBox;
        public SyntaxTextBox SyntaxTextBox
        {
            get { return m_syntaxTextBox; }
            set
            {
                m_syntaxTextBox = value;
                if (m_syntaxTextBox == null)
                    return;
                AttachToSyntaxTextBox();
            }
        }

        private void AttachToSyntaxTextBox()
        {
            SyntaxTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(SyntaxTextBox_KeyDown);
            SyntaxTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(SyntaxTextBox_KeyUp);
        }

        public class CheckAutoCompleteListIsActiveEventArgs : EventArgs { public bool IsActive { get; set; } }
        public event EventHandler<CheckAutoCompleteListIsActiveEventArgs> CheckAutoCompleteListIsActive;
        public event EventHandler ForceAutoCompleteList;
        public class ChangeAutoCompleteItemIndexEventArgs : EventArgs { public int IndexDelta { get; set; } }
        public event EventHandler<ChangeAutoCompleteItemIndexEventArgs> ChangeAutoCompleteItemIndex;
        public event EventHandler AcceptAutoCompleteAdvice;
        public event EventHandler CloseAutoCompleteList;
        public event EventHandler MakeAutoCompleteListTransparent;
        public event EventHandler MakeAutoCompleteListOpaque;

        private void SyntaxTextBox_KeyDown(object sender, KeyEventArgs e)
        {            

            if (CheckAutoCompleteListIsActive == null)
                return;

            //Die Info, ob die AutoComplete List aktiv ist via Event abholen
            var isActiveArgs = new CheckAutoCompleteListIsActiveEventArgs();
            CheckAutoCompleteListIsActive.Invoke(this, isActiveArgs);

            if (isActiveArgs.IsActive)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (ChangeAutoCompleteItemIndex != null)
                            ChangeAutoCompleteItemIndex.Invoke(this, new ChangeAutoCompleteItemIndexEventArgs() { IndexDelta = -1 });
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        if (ChangeAutoCompleteItemIndex != null)
                            ChangeAutoCompleteItemIndex.Invoke(this, new ChangeAutoCompleteItemIndexEventArgs() { IndexDelta = 1 });
                        e.Handled = true;
                        break;
                    case Keys.PageUp:
                        if (ChangeAutoCompleteItemIndex != null)
                            ChangeAutoCompleteItemIndex.Invoke(this, new ChangeAutoCompleteItemIndexEventArgs() { IndexDelta = 15 });
                        e.Handled = true;
                        break;
                    case Keys.PageDown:
                        if (ChangeAutoCompleteItemIndex != null)
                            ChangeAutoCompleteItemIndex.Invoke(this, new ChangeAutoCompleteItemIndexEventArgs() { IndexDelta = -15 });
                        e.Handled = true;
                        break;
                    case Keys.Escape:
                        if (CloseAutoCompleteList != null)
                            CloseAutoCompleteList.Invoke(this, EventArgs.Empty);
                        e.Handled = true;
                        break;
                    case Keys.Return:
                        e.SuppressKeyPress = true;
                        goto case Keys.Space;
                    case Keys.Space:
                    case Keys.OemPeriod:
                        if (AcceptAutoCompleteAdvice != null)
                            AcceptAutoCompleteAdvice.Invoke(this, EventArgs.Empty);
                        e.Handled = true;
                        break;
                }
                if (!e.Handled &&
                    (e.Modifiers & Keys.Control) == Keys.Control)
                {
                    if (MakeAutoCompleteListOpaque != null)
                        MakeAutoCompleteListOpaque.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (e.KeyCode == Keys.Space && e.Control)
                {
                    if (ForceAutoCompleteList != null)
                        ForceAutoCompleteList.Invoke(this, EventArgs.Empty);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void SyntaxTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Handled &&
                (e.Modifiers & Keys.Control) == Keys.Control)
            {
                if (MakeAutoCompleteListTransparent != null)
                    MakeAutoCompleteListTransparent.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
