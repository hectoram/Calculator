using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.Helpers
{
    public class InputParser : IInputParser
    {
        private readonly IUserFlagParser _userFlagParser;

        public InputParser(IUserFlagParser userFlagParser)
        {
            _userFlagParser = userFlagParser ?? throw new ArgumentNullException(nameof(userFlagParser));
        }

        public UserInput Parse(string paramString)
        {
            var userInputModel = _userFlagParser.Parse(paramString);

            if(paramString.Contains(Common.StartDelimiterSequence))
            {
                if(paramString.Contains(Common.OpenDelimiterChar) && paramString.Contains(Common.CloseDelimiterChar))
                {
                    userInputModel.Values = ParseMultiCharDelimiter(userInputModel.ParsedInput);
                    return userInputModel;
                }
                else
                {
                    userInputModel.Values = ParseSingleCharDelimiter(userInputModel.ParsedInput);
                    return userInputModel;
                }
            }

            if(userInputModel.Delimiters.Count == 0)
                userInputModel.Values = new List<string>(userInputModel.ParsedInput.Split(Common.DefaultDelimiters, StringSplitOptions.None));
            else
                 userInputModel.Values = new List<string>(userInputModel.ParsedInput.Split(userInputModel.Delimiters.ToArray(), StringSplitOptions.None));
            
            return userInputModel;
        }

        private List<string> ParseSingleCharDelimiter(string paramString)
        {
            var customDelimiter = paramString.Substring(2, 1)[0].ToString();            
            paramString = paramString.Substring(paramString.IndexOf(Common.NewLineSequence) + 2); // Want everything after new line which is two characters.
                
            return new List<string>(paramString.Split(new string[] { Common.DefaultDelimiterChar, Common.NewLineSequence, customDelimiter }, StringSplitOptions.None));
        }

        private List<string> ParseMultiCharDelimiter(string paramString)
        {
            var hasRemainingParameters = true;
            var customDelimiters = new List<string>() {Common.DefaultDelimiterChar, Common.NewLineSequence,};

            while(hasRemainingParameters)
            {
                var startIndex = paramString.IndexOf(Common.OpenDelimiterChar) + 1; // Do not want to include [
                var endIndex = paramString.IndexOf(Common.CloseDelimiterChar, startIndex);
                var userDelimiter = paramString.Substring(startIndex, endIndex - startIndex);

                customDelimiters.Add(userDelimiter);

                paramString = paramString.Substring(paramString.IndexOf(Common.CloseDelimiterChar) + 1); 
                if(!paramString.Contains(Common.OpenDelimiterChar) && !paramString.Contains(Common.CloseDelimiterChar))
                {
                    hasRemainingParameters = false;
                }
            }

            paramString = paramString.Substring(paramString.IndexOf(Common.NewLineSequence) + 2); // Want everything after new line which is two characters.               
            return new List<string>(paramString.Split(customDelimiters.ToArray(), StringSplitOptions.None));
        }
    }
}   