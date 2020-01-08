using System;
using System.Collections.Generic;

namespace Calculator.MathServices.Models
{
    public class UserInput
    {
        public string ParsedInput;
        public List<string> Values;
        public List<string> Delimiters;
        public bool AllowNegatives;
        public int UpperBound = int.MinValue;
        public Operation Operation;
    }
}