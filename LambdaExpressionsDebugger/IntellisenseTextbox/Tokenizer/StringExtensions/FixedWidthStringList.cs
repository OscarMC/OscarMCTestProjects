using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class FixedWidthStringList : IList<string>
    {
        private List<string> m_strings = new List<string>();

        public int Width { get; private set; }

        #region IList<string> Members

        public int IndexOf(string item)
        {
            return m_strings.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            m_strings.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_strings.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return m_strings[index];
            }
            set
            {
                m_strings[index] = value;
            }
        }

        #endregion

        #region ICollection<string> Members

        public void Add(string item)
        {
            if (m_strings.Count == 0)
                Width = item.Length;
            if (item.Length != Width)
                throw new ArgumentException("item has wrong length");
            m_strings.Add(item);
        }

        public void Clear()
        {
            m_strings.Clear();
        }

        public bool Contains(string item)
        {
            return m_strings.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            m_strings.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_strings.Count; }
        }

        public bool IsReadOnly
        {
            get { return (m_strings as ICollection<string>).IsReadOnly; }
        }

        public bool Remove(string item)
        {
            return m_strings.Remove(item);
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return m_strings.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (m_strings as System.Collections.IEnumerable).GetEnumerator();
        }

        #endregion
    }

    public static class FixedWidthStringListEnumerableExtesions 
    {
        public static FixedWidthStringList ToFixedWidthStringList<T>(this T enumerable) where T : IEnumerable<string>
        {
            FixedWidthStringList list = new FixedWidthStringList();
            foreach (var s in enumerable)
                list.Add(s);
            return list;
        }
    }
}
