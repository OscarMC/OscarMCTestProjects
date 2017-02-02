using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class Parser
    {
        public Parser(ContextProvider contextProvider)
        {
            Context = contextProvider;
            m_expressionProvider = new ExpressionProvider(Context);
        }

        private ExpressionProvider m_expressionProvider;
        private string m_lastSourceCode;
        private int m_lastCursorPos;

        public ContextProvider Context { get; set; }

        public Tokenizer Tokenizer { get; set; }

        public ParseResult Result { get; set; }

        
        public ParseResult Parse(string sourceCode, int cursorPos)
        {
            if (sourceCode == m_lastSourceCode && cursorPos == m_lastCursorPos)
                return Result;

            //var expResult = m_expressionProvider.GetExpression(sourceCode);
            //if (!expResult.CouldReturnExpression)
            //{
            //}

            if (sourceCode != m_lastSourceCode)
                Tokenizer = new Tokenizer(sourceCode);
            var token = Tokenizer.GetTokenAt(cursorPos);

            if (sourceCode != m_lastSourceCode)
                AddParseInfosToTokens();

            string advice = string.Empty;
            if (token != null)
            {
                var ci = Context[token.Text];
                if (ci != null)
                    advice = ci.Name;
            }
                        
            var result = new ParseResult()
            {
                CurrentToken = token,
                AdviceFromContextList = advice
            };

            if (!token.IsNullObject)
                result.CursorIsAtEndOfToken = (cursorPos == token.StartPos + token.Length);

            Result = result;

            m_lastSourceCode = sourceCode;
            m_lastCursorPos = cursorPos;
                        
            return result;
        }

        private void AddParseInfosToTokens()
        {
            foreach (Token token in Tokenizer.Tokens)
            {
                if (token.Text == ".")
                {
                    token.Content = TokenContent.Point;
                }
                else if (token.Text == "new")
                {
                    token.Content = TokenContent.NewKeyword;
                }
                else if (Context.KnownTypesIncludingKeywordTypes.Contains(token.Text))
                {
                    if (Context.Keywords.Contains(token.Text))
                        token.Content = TokenContent.KeywordType;
                    else
                        token.Content = TokenContent.Type;
                }
                else
                {
                    if (token.ElementType == TokenElementType.TextElement)
                    {
                        var ci = Context[token.Text, ContextSearchMode.Exact];
                        if (ci != null)
                        {
                            if (ci.SymbolType == SymbolType.Property ||
                                ci.SymbolType == SymbolType.Field ||
                                ci.SymbolType == SymbolType.Constant)
                                token.Content = TokenContent.TypeInstance;
                        }
                    }
                }
            }
        }

        public List<ContextItem> GetAutoCompleteListItems()
        {
            List<ContextItem> items;

            if (KnownPatterns.IsInSubItemsListPosition.Check(Result))
            {
                items = GetSubItemList();
            }
            else
            {
                items = Context.Items;
            }

            return items;
        }

        private List<ContextItem> GetSubItemList()
        {
            string typeName = Result.CurrentToken.Previous.Text;
            ContextItem ci = Context[typeName];

            List<ContextItem> items = new List<ContextItem>();

            if (ci.IsType) //Statics eines Typs anzeigen
            {
                items = ContextProvider.GetStaticMembers(ci.Type, Context.Type).ToList();
            }
            else if (ci.IsMember) //Zugreifbare Members eines vorausgehenden Members anzeigen
            {
                items = ContextProvider.GetInstanceMembers(ci.Type, Context.Type).ToList();
            }

            return items;
        }
    }
}
