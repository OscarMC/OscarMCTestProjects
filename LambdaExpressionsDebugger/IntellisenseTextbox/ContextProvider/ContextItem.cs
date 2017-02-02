using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IntellisenseTextbox
{
    public class ContextItem
    {
        public SymbolType SymbolType { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        private string m_loweredName;
        public string LoweredName
        {
            get
            {
                if (m_loweredName == null)
                    m_loweredName = Name.ToLower();
                return m_loweredName;
            }
        }

        public bool IsType
        {
            get
            {
                return 
                    SymbolType == SymbolType.Class || 
                    SymbolType == SymbolType.Delegate || 
                    SymbolType == SymbolType.Interface;
            }
        }

        public bool IsMember
        {
            get
            {
                return 
                    SymbolType == SymbolType.Constant || 
                    SymbolType == SymbolType.Field || 
                    SymbolType == SymbolType.Property || 
                    SymbolType == SymbolType.Method;
            }
        }

        public List<MethodInfo> MethodOverloads { get; set; }

        private Dictionary<int, string> m_loweredFirstChars = new Dictionary<int, string>();
        public string GetLoweredFirstCharsCached(int nrOfChars)
        {
            if (!m_loweredFirstChars.ContainsKey(nrOfChars))
                m_loweredFirstChars.Add(nrOfChars, LoweredName.PadRight(nrOfChars).Substring(0, nrOfChars));
            return m_loweredFirstChars[nrOfChars];
        }

        public override string ToString()
        {
            return Name;
        }

        public class NameComparer : IEqualityComparer<ContextItem>
        {
            #region IEqualityComparer<ContextItem> Members

            public bool Equals(ContextItem x, ContextItem y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(ContextItem obj)
            {
                return obj.Name.GetHashCode();
            }

            #endregion
        }
        
    }
}
