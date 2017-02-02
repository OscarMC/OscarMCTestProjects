using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public enum TokenElementType 
    { 
        TextElement, 
        SeperatorElement 
    }

    public enum TokenContent
    {
        Undefined,
        Namespace, 
        Keyword, 
        KeywordType,
        Type, 
        TypeInstance, 
        Point, 
        NewKeyword
    }

    public class Token
    {
        private string m_text = string.Empty;
        public string Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                UpdateLength();
            }
        }

        public int StartPos { get; set; }        
        public int Length { get; set; }
        public TokenElementType ElementType
        {
            get
            {
                if (Tokenizer.Seperators.Contains(Text))
                    return TokenElementType.SeperatorElement;
                else
                    return TokenElementType.TextElement;
            }
        }

        public Token Previous
        {
            get
            {
                Token t = PreviousIncludingCommentsAndSpaces;
                while (t != null && (t.IsComment || t.IsSpace))
                    t = t.PreviousIncludingCommentsAndSpaces;
                return t;
            }
        }
        public Token Next
        {
            get
            {
                Token t = NextIncludingCommentsAndSpaces;
                while (t != null && (t.IsComment || t.IsSpace))
                    t = t.NextIncludingCommentsAndSpaces;
                return t;
            }
        }

        public Token PreviousIncludingCommentsAndSpaces { get; set; }
        public Token NextIncludingCommentsAndSpaces { get; set; }

        public bool IsSpace { get { return Text == " " || Text == "\t"; } }
        public bool IsComment { get { return Text.StartsWith("//") || Text.StartsWith("/*"); } }
        public bool IsStringConstant { get { return Text.StartsWith("\"") || Text.StartsWith("@\"") || Text.StartsWith("'"); } }
        public bool IsKeyword { get { return Content == TokenContent.Keyword || Content == TokenContent.KeywordType || Content == TokenContent.NewKeyword; } }
        public bool IsKnownType { get { return Content == TokenContent.Type; } }        

        private bool m_isNullObject = false;
        public bool IsNullObject { get { return m_isNullObject; } }

        public int LeftSpaces
        {
            get
            {
                int count = 0;
                Token t = Previous;
                while (t != null && t.Text == " ")
                {
                    count++;
                    t = t.Previous;
                }
                return count;
            }
        }

        public int RightSpaces
        {
            get
            {
                int count = 0;
                Token t = Next;
                while (t != null && t.Text == " ")
                {
                    count++;
                    t = t.Next;
                }
                return count;
            }
        }

        private string m_loweredText;
        public string LoweredText
        {
            get
            {
                if (m_loweredText == null)
                    m_loweredText = Text.ToLower();
                return m_loweredText;
            }
        }

        public TokenContent Content { get; set; }

        private void UpdateLength()
        {
            if (m_text.Length != Length)
            {
                int diff = m_text.Length - Length;
                Length = m_text.Length;
                if (NextIncludingCommentsAndSpaces != null)
                    NextIncludingCommentsAndSpaces.UpdateStartPos(diff);
            }
        }

        private void UpdateStartPos(int diff)
        {
            StartPos += diff;
            if (NextIncludingCommentsAndSpaces != null)
                NextIncludingCommentsAndSpaces.UpdateStartPos(diff);
        }


        public override string ToString()
        {
            return Text.PadRight(30) + " - " + new { Text, StartPos, Length }.ToString();
        }

    }
}
