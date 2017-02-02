using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntellisenseTextbox
{
    public partial class SyntaxTextBox : RichTextBox
    {
        public SyntaxTextBox()
        {
            InitializeComponent();                        
        }

        private bool m_isPainting = true;
        private bool m_suppressingTextChangedEvent = false;        

        public class NeedTokenAtCursorPosEventArgs : EventArgs 
        {
            public NeedTokenAtCursorPosEventArgs(string text, int cursorPos)
            {
                Text = text;
                CursorPos = cursorPos;
            }
            public string Text { get; private set; }
            public int CursorPos { get; private set; }
            public Token Token { get; set; } 
        }
        public event EventHandler<NeedTokenAtCursorPosEventArgs> NeedTokenAtCursorPos;


        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x00f)
            {
                if (m_isPainting)
                    base.WndProc(ref m);
                else
                    m.Result = IntPtr.Zero;
            }
            else
                base.WndProc(ref m);
        }

        protected override void OnTextChanged(EventArgs e)
        {            
            if (NeedTokenAtCursorPos == null)
                return;
                        
            var getTokenArgs = new NeedTokenAtCursorPosEventArgs(Text, 0);
            NeedTokenAtCursorPos.Invoke(this, getTokenArgs);            

            m_isPainting = false;

            int selStart = SelectionStart;
            int selLen = SelectionLength;
                       

            Token token = getTokenArgs.Token;
            while (token != null)
            {
                Select(token.StartPos, token.Length);
                if (token.IsKeyword)
                    SelectionColor = Color.Blue;
                else if (token.IsKnownType)
                    SelectionColor = Color.FromArgb(43, 145, 175);
                else if (token.IsComment)
                    SelectionColor = Color.ForestGreen;
                else if (token.IsStringConstant)
                    SelectionColor = Color.DarkRed;
                else
                    SelectionColor = Color.Black;
                token = token.NextIncludingCommentsAndSpaces;
            }
            SelectionStart = selStart;
            SelectionLength = selLen;

            m_isPainting = true;

            if (!m_suppressingTextChangedEvent)
            {                
                base.OnTextChanged(e);
            }

        }

        internal void SetTextWithoutEvents(string text)
        {
            m_suppressingTextChangedEvent = true;
            Text = text;
            m_suppressingTextChangedEvent = false;
        }
    }
}
