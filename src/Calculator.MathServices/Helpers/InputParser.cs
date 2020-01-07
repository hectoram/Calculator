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
                if(paramString.Contains("[") && paramString.Contains("]"))
                    return ParseMultiCharDelimiter(paramString);
                else
                    return ParseSingleCharDelimiter(paramString);
            }

            //Simple case of comma and new line parsing
            var parsedArray = paramString.Split(new string[] { ",", @"\n" }, StringSplitOptions.None);
            return parsedArray;
        }

        private string[] ParseSingleCharDelimiter(string paramString)
        {
            var customDelimiter = paramString.Substring(2, 1)[0].ToString();            
            paramString = paramString.Substring(paramString.IndexOf(@"\n") + 2); // Want everything after new line which is two characters.
                
            return paramString.Split(new string[] { ",", @"\n", customDelimiter }, StringSplitOptions.None);
        }

        private string[] ParseMultiCharDelimiter(string paramString)
        {
            var startIndex = paramString.IndexOf('[') + 1; // Do not want to include [
            var endIndex = paramString.IndexOf("]", startIndex);
            var customDelimiter = paramString.Substring(startIndex, endIndex - startIndex);
  
            paramString = paramString.Substring(paramString.IndexOf(@"\n") + 2); // Want everything after new line which is two characters.               
            return paramString.Split(new string[] { ",", @"\n", customDelimiter }, StringSplitOptions.None);
        }
    }
}   