using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    class ExpressionErrorCollection
    {
        private List<ExpressionError> m_errors = new List<ExpressionError>();
        internal List<ExpressionError> Errors
        {
            get { return m_errors; }
            set { m_errors = value; }
        }

        public bool HasErrors { get { return Errors.Count > 0; } }

        public string CompleteErrorText
        {
            get
            {
                return string.Concat((from e in Errors 
                                      select e.Text + Environment.NewLine)
                                      .ToArray());
            }
        }
    }
}
