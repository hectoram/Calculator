using System;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.Helpers
{
    public static class Common
    {
        public const string StartDelimiterSequence = "//";
        public const string OpenDelimiterChar = "[";
        public const string CloseDelimiterChar = "]";
        public const string DefaultDelimiterChar = ",";
        public const string NewLineSequence = @"\n";
        public const string DelimiterFlag = "-d=";
        public const string UpperBoundFlag = "-b=";
        public const string NegativeAllowanceFlag = "-n=";
        public const string OperationFlag = "-n=";
        public const string EndCustomParametersSequence = "||";

        public static string[] DefaultDelimiters = new string[]{@"\n", ","};
        public static bool DefaultAllowNegatives = false;
        public static Operation DefaultOperation = Operation.Add;
        public static int DefaultUpperBound = 1000;
    }
}