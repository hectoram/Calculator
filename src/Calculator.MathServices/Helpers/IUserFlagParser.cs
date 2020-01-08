using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.Helpers
{
    public interface IUserFlagParser
    {
        UserInput Parse(string paramString);
    }
}