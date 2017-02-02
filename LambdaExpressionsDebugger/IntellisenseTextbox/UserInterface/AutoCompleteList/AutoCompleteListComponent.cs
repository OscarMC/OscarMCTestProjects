using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace IntellisenseTextbox
{
    public partial class AutoCompleteListComponent : Component
    {
        public AutoCompleteListComponent()
        {
            CreateAutoCompleteList();
            InitializeComponent();
        }

        public AutoCompleteListComponent(IContainer container)
        {
            container.Add(this);

            CreateAutoCompleteList();

            InitializeComponent();
        }

        private void CreateAutoCompleteList()
        {
            AutoCompleteList = new AutoCompleteList();
        }

        private AutoCompleteList AutoCompleteList { get; set; }

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

        public Point AutoCompleteListPosition
        {
            get
            {
                var autoCompletePos = SyntaxTextBox.PointToScreen(RelativeAutoCompleteListPos);
                return autoCompletePos;
            }
        }

        public Point RelativeAutoCompleteListPos
        {
            get
            {
                int charWidth = 6;
                Point cursorPos = SyntaxTextBox.GetPositionFromCharIndex(SyntaxTextBox.SelectionStart);
                Point autoCompletePos = new Point() { X = cursorPos.X - 20 * charWidth + 90, Y = cursorPos.Y + AutoCompleteList.Font.Height + 7 };                
                return autoCompletePos;
            }
        }


        public class NeedCurrentTokenEventArgs : EventArgs { public Token Token { get; set; } }
        public event EventHandler<NeedCurrentTokenEventArgs> NeedCurrentToken;
        public class PopulateAutoCompleteListEventArgs : EventArgs { public List<ContextItem> Items { get; set; } }
        public event EventHandler<PopulateAutoCompleteListEventArgs> PopulateAutoCompleteList;
        public class NeedCurrentParseResultEventArgs : EventArgs { public ParseResult Result { get; set; } }
        public event EventHandler<NeedCurrentParseResultEventArgs> NeedCurrentParseResult;

        private void AttachToSyntaxTextBox()
        {
            SyntaxTextBox.MouseWheel += new MouseEventHandler(SyntaxTextBox_MouseWheel);
            SyntaxTextBox.TextChanged += new EventHandler(SyntaxTextBox_TextChanged);
            keyHandler.SyntaxTextBox = SyntaxTextBox;
            if (AutoCompleteList != null)
                AutoCompleteList.WindowToShowToolTipsOn = SyntaxTextBox;       
        }

        private void SyntaxTextBox_TextChanged(object sender, EventArgs e)
        {            
            UpdateAutoCompleteList();
        }

        private void UpdateAutoCompleteList()
        {
            UpdateAutoCompleteList(false);
        }

        private void SyntaxTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            AutoCompleteList.ChangeAutoCompleteItemIndex((e.Delta > 0 ? -1 : 1));
        }

        private void keyHandler_ChangeAutoCompleteItemIndex(object sender, SyntaxTextBoxKeyHandler.ChangeAutoCompleteItemIndexEventArgs e)
        {
            AutoCompleteList.ChangeAutoCompleteItemIndex(e.IndexDelta);
        }

        private void keyHandler_CheckAutoCompleteListIsActive(object sender, SyntaxTextBoxKeyHandler.CheckAutoCompleteListIsActiveEventArgs e)
        {
            e.IsActive = AutoCompleteList.Visible;
        }

        private void keyHandler_CloseAutoCompleteList(object sender, EventArgs e)
        {
            AutoCompleteList.Hide();
        }

        private void keyHandler_MakeAutoCompleteListTransparent(object sender, EventArgs e)
        {
            AutoCompleteList.Opacity = 0.2f;
        }

        private void keyHandler_MakeAutoCompleteListOpaque(object sender, EventArgs e)
        {
            AutoCompleteList.Opacity = 0.9f;
        }

        private void keyHandler_AcceptAutoCompleteAdvice(object sender, EventArgs e)
        {
            if (AutoCompleteList.HasAdvice &&
                NeedCurrentToken != null)
            {
                int currentCursorPos = SyntaxTextBox.SelectionStart;
                string advice = AutoCompleteList.Advice;

                var currentTokenArgs = new NeedCurrentTokenEventArgs();
                NeedCurrentToken.Invoke(this, currentTokenArgs);

                int tokenStartPos = currentTokenArgs.Token.StartPos;

                if (currentTokenArgs.Token.Text != ".")
                {
                    SyntaxTextBox.SetTextWithoutEvents(SyntaxTextBox.Text.Substring(0, tokenStartPos) + advice);
                    SyntaxTextBox.SelectionLength = 0;
                    SyntaxTextBox.SelectionStart = tokenStartPos + advice.Length;
                }
                else
                {
                    SyntaxTextBox.SetTextWithoutEvents(SyntaxTextBox.Text + advice);
                    SyntaxTextBox.SelectionLength = 0;
                    SyntaxTextBox.SelectionStart = tokenStartPos + 1 + advice.Length;

                }
            }
        }

        private void keyHandler_ForceAutoCompleteList(object sender, EventArgs e)
        {
            UpdateAutoCompleteList(true);
        }

        private void UpdateAutoCompleteList(bool forceShow)
        {
            if (NeedCurrentParseResult == null) return;

            ParseResult parseResult = GetParseResults();           

            UpdateItemList();

            bool isShowing = GetIsShowing(parseResult, forceShow);

            if (!isShowing)
            {
                AutoCompleteList.Hide();
                return;
            }

            UpdateAutoCompleteList(parseResult);
            
        }

        private void UpdateItemList()
        {
            if (PopulateAutoCompleteList == null) return;

            AutoCompleteList.ApplyItemList(GetListItems());
        }

        public void PreLoadItemList()
        {
            AutoCompleteList.ApplyItemList(GetListItems());
            AutoCompleteList.Opacity = 0f;
            AutoCompleteList.Show();
            AutoCompleteList.Hide();
            AutoCompleteList.Opacity = 0.9f;
        }

        public List<ContextItem> GetListItems()
        {            
            if (PopulateAutoCompleteList == null) return new List<ContextItem>();

            var autoCompleteListArgs = new PopulateAutoCompleteListEventArgs() { Items = new List<ContextItem>() };            
            PopulateAutoCompleteList.Invoke(this, autoCompleteListArgs);            
            return autoCompleteListArgs.Items;
        }

        private ParseResult GetParseResults()
        {
            if (NeedCurrentParseResult == null) return null;
            var parseResultArgs = new NeedCurrentParseResultEventArgs();
            NeedCurrentParseResult.Invoke(this, parseResultArgs);
            return parseResultArgs.Result;
        }

        private bool GetIsShowing(ParseResult parseResult, bool forceShow)
        {
            return
                AutoCompleteList.HasItems &&
                (forceShow || KnownPatterns.IsAutoCompleteListSituation.Check(parseResult));
        }

        private int FindBestMatchingItemIndex(Token token)
        {            
            return GetListItems().FindIndex(i => i.LoweredName.StartsWith(token.LoweredText));            
        }

        private void UpdateAutoCompleteList(ParseResult parseResult)
        {
            if (!AutoCompleteList.HasItems) return;
                        
            var token = parseResult.CurrentToken;
            int idxOfMatchingOne = FindBestMatchingItemIndex(token);

            AutoCompleteList.SelectIndex(idxOfMatchingOne);

            AutoCompleteList.UpperLeftCornerRelativeToToolTipParent = RelativeAutoCompleteListPos;
            AutoCompleteList.ShowAtPosition(AutoCompleteListPosition);            
            SyntaxTextBox.Focus();
        }

    }
}
