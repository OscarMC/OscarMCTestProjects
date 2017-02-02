using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class Tokenizer
    {
        public Tokenizer(string textToTokenize)
        {
            m_textToTokenize = textToTokenize;
        }

        public static string[] Seperators =
            new string[] 
            { 
                " is ", " as ",
                "<<=", ">>=", 
                "==", "!=", ">=", "<=", "&&", "||", "=>", "++", "--", ">>", "<<", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "??",
                ".", ",", ";", "(", ")", "[", "]", "{", "}", ":", "-", "+", "*", "~", "^", "!", "%", "&", "/", "=", "?", "<", ">", "|", "\r", "\n", "\t", " ",
                TextFridge.SpecialSeperatorString //<- für TextFridge
            };


        private string m_textToTokenize;
        private Dictionary<string, string> m_stringConstantFridge = new Dictionary<string, string>();
        private List<Token> m_tokens;

        public List<Token> Tokens
        {
            get
            {
                if (m_tokens == null)
                    Tokenize();
                return m_tokens;
            }
        }

        internal Token GetTokenAt(int cursorPos)
        {
            Token result = Tokens.FirstOrDefault(t => t.StartPos <= cursorPos && t.StartPos + t.Length >= cursorPos);
            return (result ?? new Token());
        }

        private void Tokenize()
        {
            ClearTokens();

            #region störende Textregionen konservieren

            TextFridge fridge = new TextFridge(m_textToTokenize);
            fridge.EscapeSequences = new List<string>() { "\\\"", "\\\'" };

            //Strings, Char-Konstanten, Single- und Multi-Line-Kommentare
            fridge.RegisterTextRegion("\"");            
            fridge.RegisterTextRegion("'");
            fridge.RegisterTextRegion(new[] { "//" }, new[] { "\n", "\r" }, true, false);
            fridge.RegisterTextRegion("/*", "*/");

            string cleanedText = fridge.GetFrozenText();

            #endregion

            //String splitten
            List<string> splittedText = cleanedText.Split(Seperators);

            //Tokenobjekte erzeugen
            int pos = 0;
            foreach (string s in splittedText)
            {
                if (s == TextFridge.SpecialSeperatorString) continue;
                m_tokens.Add(
                    new Token()
                    {
                        Text = s,
                        StartPos = pos,
                        Length = s.Length
                    });
                pos += s.Length;
            }

            //Die Tokenobjekte verketten
            int idx;
            idx = 0;
            m_tokens.ForEach(t => { t.PreviousIncludingCommentsAndSpaces = (idx > 0 ? m_tokens[idx - 1] : null); idx++; });
            idx = 0;
            m_tokens.ForEach(t => { t.NextIncludingCommentsAndSpaces = (idx < m_tokens.Count - 1 ? m_tokens[idx + 1] : null); idx++; });

            //Die konservierten Textregionen wieder einfügen
            foreach (Token t in m_tokens)
                t.Text = fridge.DefrostIfFrozen(t.Text);
        }


        private void ClearTokens()
        {
            m_tokens = new List<Token>();
        }
    }
}
