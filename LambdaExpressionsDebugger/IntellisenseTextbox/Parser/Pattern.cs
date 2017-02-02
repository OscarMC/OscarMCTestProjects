using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntellisenseTextbox
{
    public class Pattern<T>
    {
        public Predicate<T> Check { get; set; }        
    }
}
