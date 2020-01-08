using System;
using System.Collections.Generic;
using System.Text;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices
{
    public class MathService : IMathService
    {
        private readonly IInputParser _parser;
        private int _maxValue = 1000;
        private List<int> _negativeNumbers;

        public MathService(IInputParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _negativeNumbers = new List<int>();
        }
        
        public string Add(string paramString)
        {
            _negativeNumbers.Clear();

            var userInput = _parser.Parse(paramString);
            var total = GetSum(userInput);

            if(!userInput.AllowNegatives && _negativeNumbers.Count > 0)
                throw new ArgumentException($"Negative Numbers Were Provided: {String.Join(",", _negativeNumbers)}");

            return total.ToString();
        }

        private string GetSum(UserInput userInput)
        {
            var total = 0;
            var sb = new StringBuilder();
            var upperLimit = _maxValue;

            if(userInput.UpperBound != int.MinValue)
                upperLimit = userInput.UpperBound;

            foreach(string i in userInput.Values)
            {
                if(!string.IsNullOrWhiteSpace(i) && Int32.TryParse(i, out int parsedValue))
                {
                    if (parsedValue >= 0)
                    {
                        if(parsedValue <= upperLimit)
                        {
                            sb.Append($"{i.ToString()}+");
                            total = total + parsedValue;   
                        }else
                        {
                            sb.Append("0+");
                        }
                    }else
                    {
                        sb.Append("0+");
                        _negativeNumbers.Add(parsedValue);
                    } 
                }else
                {
                    sb.Append("0+");
                }
            }
            sb.Length--; // Remove last + sign

            return $"{sb} = {total.ToString()}";
        }
    }
}