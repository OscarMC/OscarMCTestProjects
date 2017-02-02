using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace IntellisenseTextbox
{
    class ExpressionResult
    {
        public Expression<Action> Expression { get; set; }
        
        public bool HasErrors { get { return Errors.HasErrors; } }
        public bool Successful { get { return !HasErrors; } }
        public bool CouldReturnExpression { get { return Expression != null; } }
        
        public ExpressionErrorCollection Errors { get; set; }
    }
}
