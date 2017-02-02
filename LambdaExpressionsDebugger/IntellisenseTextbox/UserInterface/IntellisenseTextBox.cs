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
    public partial class IntellisenseTextBox : UserControl
    {
        public IntellisenseTextBox()
        {
            InitializeComponent();
            ContextType = this.GetType();            
        }

        private void OnContextTypeChanged()
        {
            ContextProvider = new ContextProvider(ContextType);
            Parser = new Parser(ContextProvider);
            autoCompleteList.PreLoadItemList();
        }

        private Type m_contextType;
        public Type ContextType
        {
            get { return m_contextType; }
            set
            {
                m_contextType = value;
                OnContextTypeChanged();
            }
        }

        public ContextProvider ContextProvider {get;set;}
        public Parser Parser {get;set;}

        public override string Text
        {
            get { return syntaxTextBox.Text; }
            set { syntaxTextBox.Text = value; }
        }

        public int CursorPos
        {
            get { return syntaxTextBox.SelectionStart; }
        }

        public event EventHandler SourceCodeChanged;

        private void syntaxTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SourceCodeChanged != null)
                SourceCodeChanged.Invoke(this, EventArgs.Empty);
        }

        private void syntaxTextBox_NeedTokenAtCursorPos(object sender, SyntaxTextBox.NeedTokenAtCursorPosEventArgs e)
        {
            Parser.Parse(e.Text, e.CursorPos);
            e.Token = Parser.Result.CurrentToken;
        }

        private void autoCompleteList_NeedCurrentTokenStartPos(object sender, AutoCompleteListComponent.NeedCurrentTokenEventArgs e)
        {
            Parser.Parse(Text, CursorPos);
            e.Token = Parser.Result.CurrentToken;
        }

        private void autoCompleteList_PopulateAutoCompleteList(object sender, AutoCompleteListComponent.PopulateAutoCompleteListEventArgs e)
        {
            Parser.Parse(Text, CursorPos);
            e.Items = Parser.GetAutoCompleteListItems();
        }

        private void autoCompleteList_NeedCurrentParseResult(object sender, AutoCompleteListComponent.NeedCurrentParseResultEventArgs e)
        {
            e.Result = Parser.Parse(Text, CursorPos);
        }
        
    }
}
