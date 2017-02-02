using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public enum SymbolType : int
    {
        Namespace = 0,
        Class = 1,
        Interface = 2,
        Method = 3,
        Property = 4,
        Field = 5,
        Constant = 6,        
        Enum = 7,
        EnumItem = 8,
        Delegate = 9,
        Event = 10,
        Keyword = 11,
        ExtensionMethod = 12,
        GenericParameter = 13
    }
}
