using System;
using System.Collections.Generic;
using Calculator.MathServices.Helpers;

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

            var parsedArray = _parser.Parse(paramString);
            var total = GetSum(parsedArray);

            if(_negativeNumbers.Count > 0)
                throw new ArgumentException($"Negative Numbers Were Provided: {String.Join(",", _negativeNumbers)}");

            return total.ToString();
        }

        private string GetSum(string[] parsedArray)
        {
            var total = 0;

            foreach(string i in parsedArray)
            {
                if(!string.IsNullOrWhiteSpace(i) && Int32.TryParse(i, out int parsedValue))
                {
                    if (parsedValue >= 0)
                    {
                        if(parsedValue <= _maxValue)
                            total = total + parsedValue;   
                    }else
                    {
                        _negativeNumbers.Add(parsedValue);
                    } 
                }
            }
            return total.ToString();
        }
    }
}