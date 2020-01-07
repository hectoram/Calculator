using System;
using System.Collections.Generic;

namespace Calculator.MathServices.Helpers
{
    public class InputParser : IInputParser
    {
        public string[] Parse(string paramString)
        {
            if(paramString.Contains("//"))
            {
                return ParseSingleCharDelimiter(paramString);
            }
            
            //Simple case of comma and new line parsing
            var parsedArray = paramString.Split(new string[] { ",", @"\n" }, StringSplitOptions.None);
            return parsedArray;
        }

        private string[] ParseSingleCharDelimiter(string paramString)
        {
            var customDelimiter = paramString.Substring(2, 1)[0].ToString();

                int index = paramString.IndexOf(@"\n");
                paramString = paramString.Substring(index + 2); // Want everything after new line which is two characters.
                    
                return paramString.Split(new string[] { ",", @"\n", customDelimiter }, StringSplitOptions.None);
        }
    }
}   