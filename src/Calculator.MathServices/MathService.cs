using System;
using System.Collections.Generic;

namespace Calculator.MathServices
{
    public class MathService : IMathService
    {
        private int _maxValue = 1000;
        public string Add(string paramString)
        {
            var parsedList = ProccessUserInput(paramString);
            var negativeNumbers = new List<int>();

            var total = 0;

            foreach(string i in parsedList)
            {
                if(!string.IsNullOrWhiteSpace(i) && Int32.TryParse(i, out int parsedValue))
                {
                    if (parsedValue >= 0)
                    {
                        if(parsedValue <= _maxValue)
                            total = total + parsedValue;   
                    }else
                    {
                        negativeNumbers.Add(parsedValue);
                    } 
                }
            }

            if(negativeNumbers.Count > 0)
                throw new ArgumentException($"Negative Numbers Were Provided: {String.Join(",", negativeNumbers)}");

            return total.ToString();
        }

        private string[] ProccessUserInput(string paramString)
        {
            var parsedList = paramString.Split(new string[] { ",", @"\n" }, StringSplitOptions.None);
            return parsedList;
        }
    }
}