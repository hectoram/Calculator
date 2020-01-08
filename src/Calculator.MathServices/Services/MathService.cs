using System;
using System.Collections.Generic;
using System.Text;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices
{
    public class MathService : IMathService
    {
        private readonly int _maxValue = 1000;
        private readonly List<int> _negativeNumbers;
        private readonly string _zeroValue = "0";
        private readonly Dictionary<Operation, string> _operationStrings;

        private readonly IInputParser _parser;

        public MathService(IInputParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _negativeNumbers = new List<int>();
            _operationStrings = new Dictionary<Operation, string>
            {
                {Operation.Add, "+"},
                {Operation.Sub, "-"},
                {Operation.Mul, "*"},
                {Operation.Div, "/"}
            };
        }
        
        public string Process(string paramString)
        {
            _negativeNumbers.Clear();

            var userInput = _parser.Parse(paramString);
            var total = GetTotal(userInput);

            if(!userInput.AllowNegatives && _negativeNumbers.Count > 0)
                throw new ArgumentException($"Negative Numbers Were Provided: {String.Join(",", _negativeNumbers)}");

            return total.ToString();
        }

        private string GetTotal(UserInput userInput)
        {
            var total = 0;
            var sb = new StringBuilder();
            var upperLimit = _maxValue;
            var isFirstRun = true;

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
                            sb.Append($"{i.ToString()}{_operationStrings[userInput.Operation]}");                       
                            if(isFirstRun && userInput.Operation != Operation.Add)
                            {
                                total = ProcessTotal(Operation.Add, total, parsedValue);
                                isFirstRun = false;
                            } else
                            {
                                total = ProcessTotal(userInput.Operation, total, parsedValue); 
                            }
                        }else
                        {
                            sb.Append($"{_zeroValue}{_operationStrings[userInput.Operation]}");
                        }
                    }else
                    {
                        sb.Append($"{_zeroValue}{_operationStrings[userInput.Operation]}");
                        _negativeNumbers.Add(parsedValue);
                    } 
                }else
                {
                    sb.Append($"{_zeroValue}{_operationStrings[userInput.Operation]}");
                }
            }
            sb.Length--; // Remove last + sign

            return $"{sb} = {total.ToString()}";
        }

    private int ProcessTotal(Operation userOp, int currentTotal, int nextNumber)
    {
        switch (userOp)
        {
            case  Operation.Add:
                return currentTotal + nextNumber;
            case Operation.Sub:
                return currentTotal - nextNumber;
            case Operation.Mul:
                return currentTotal * nextNumber;
            case Operation.Div:
                return currentTotal / nextNumber;
            default:
                return 0;
        }
    }
    }
}