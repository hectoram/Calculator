using System;

namespace Calculator.MathServices.Helpers
{
    public interface IInputParser
    {
        string[] Parse(string paramString);
    }
}