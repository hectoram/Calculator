using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.Helpers
{
    public class UserFlagParser : IUserFlagParser
    {
        private readonly int _flagSize = 3;
        public UserInput Parse(string paramString)
        {
            var result = new UserInput();
            result.Delimiters = new List<string>();
            result.Values = new List<string>();
            result.UpperBound = Common.DefaultUpperBound;

            if(paramString.Contains(Common.EndCustomParametersSequence))
                ProcessUserFlags(paramString, result);
            else
                result.ParsedInput = paramString;

            return result;
        }

        //Flags Possible -d (delimiter) -o (Opperation) -b (UpperBound) -n (Negative Allowance T|F)
        //Following pattern is now reserved || as end of custom ops :)
        private void ProcessUserFlags(string paramString, UserInput result)
        {
            if(paramString.Contains(Common.DelimiterFlag))
            {
                var startIndex = paramString.IndexOf(Common.DelimiterFlag);
                var userDelimiter = paramString[startIndex + _flagSize];

                result.Delimiters = new List<string>{Common.DefaultDelimiterChar, userDelimiter.ToString()};
                paramString = RemoveFlagFromUserInput(paramString, $"{Common.DelimiterFlag}{userDelimiter}", string.Empty);
            }

            if(paramString.Contains(Common.NegativeAllowanceFlag))
            {
                var startIndex = paramString.IndexOf(Common.NegativeAllowanceFlag);
                var allowNegatives = paramString[startIndex + _flagSize];

                result.AllowNegatives = ToBoolean(allowNegatives.ToString());
                paramString = RemoveFlagFromUserInput(paramString, $"{Common.NegativeAllowanceFlag}{allowNegatives}", string.Empty);
            }

            if(paramString.Contains(Common.UpperBoundFlag))
            {

                var startIndex = paramString.IndexOf(Common.EndCustomParametersSequence);
                var opsString = paramString.Substring(0, startIndex);
                var userBounds = Regex.Match(opsString, @"\d+").Value;

                if(!Int32.TryParse(userBounds, out result.UpperBound))
                    result.UpperBound = Common.DefaultUpperBound;

                paramString = RemoveFlagFromUserInput(paramString, $"{Common.UpperBoundFlag}{result.UpperBound}", string.Empty);
            }
            
            if(paramString.Contains(Common.OperationFlag))
            {
                var startIndex = paramString.IndexOf(Common.EndCustomParametersSequence);
                var opsString = paramString.Substring(0, startIndex);

                if(opsString.IndexOf(Operation.Sub.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    result.Operation = Operation.Sub;

                if(opsString.IndexOf(Operation.Mul.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    result.Operation = Operation.Mul;

                if(opsString.IndexOf(Operation.Div.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    result.Operation = Operation.Div;
            }

            var removeIndex = paramString.IndexOf(Common.EndCustomParametersSequence);           
            result.ParsedInput =  paramString.Substring(removeIndex + 2); // Remove both || characters
        }

        private string RemoveFlagFromUserInput(string inputString, string replacePattern, string replacement)
        {
            return inputString.Replace(replacePattern, replacement);
        }
                private bool ToBoolean(string value)
        {
            switch (value.ToLower())
            {
                case  "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    return false;
            }
        }
    }
}