using System;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.Helpers
{
    public interface IInputParser
    {
        UserInput Parse(string paramString);
    }
}