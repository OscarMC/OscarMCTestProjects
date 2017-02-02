using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    enum ErrorType { BuildError, ExecutionError }
    class ExpressionError
    {
        public ErrorType ErrorType { get; set; }
        public string Text { get; set; }
    }
}
