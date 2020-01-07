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
            var hasRemainingParameters = true;
            var customDelimiters = new List<string>() {",", @"\n",};

            while(hasRemainingParameters)
            {
                var startIndex = paramString.IndexOf('[') + 1; // Do not want to include [
                var endIndex = paramString.IndexOf("]", startIndex);
                var userDelimiter = paramString.Substring(startIndex, endIndex - startIndex);

                customDelimiters.Add(userDelimiter);

                paramString = paramString.Substring(paramString.IndexOf("]") + 1); 
                if(!paramString.Contains("[") && !paramString.Contains("]"))
                {
                    hasRemainingParameters = false;
                }
            }

            paramString = paramString.Substring(paramString.IndexOf(@"\n") + 2); // Want everything after new line which is two characters.               
            return paramString.Split(customDelimiters.ToArray(), StringSplitOptions.None);
        }
    }
}   