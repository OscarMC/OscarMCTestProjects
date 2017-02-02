using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class ParseResult
    {
        public Type ConstrainingParentType { get; set; }
        public Token CurrentToken { get; set; }
        public bool CursorIsAtEndOfToken { get; set; }
        public string AdviceFromContextList { get; set; }
    }
}
