using System;
using System.Collections.Generic;

namespace Calculator.MathServices
{
    public class MathService : IMathService
    {
        public string Add(string paramString)
        {
            var parsedList = ProccessUserInput(paramString);
            var total = 0;

            foreach(string i in parsedList)
            {
                if (Int32.TryParse(i, out int j))
                    total = total + j;    
            }

            return total.ToString();
        }

        private string[] ProccessUserInput(string paramString)
        {
            var parsedList = paramString.Split(',');
            
            if(parsedList.Length > 2)
            {
                throw new ArgumentException();
            }

            return parsedList;
        }
    }
}