using System;
using Xunit;
using Moq;

using Calculator.MathServices;
using Calculator.MathServices.Helpers;

namespace Calculator.MathServices.UnitTests
{
    public class InputParserUnitTests
    {
        private readonly IInputParser _parser;
        public InputParserUnitTests()
        {
            _parser = new InputParser();
        }

        [Theory]
        [InlineData("50",1)]
        [InlineData("50,3",2)]
        [InlineData(@"50\n3",2)]
        [InlineData(@"50\n3\n4\n1",4)]
        [InlineData("50,3,4",3)]
        public void Parse_Returns_CorrectCount_For_CommaAndNewLine(string paramString, int expectedCount)
        {
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Length);
        }

        [Theory]
        [InlineData(@"//#\n2#5#4",3)]
        [InlineData(@"//#\n2#5", 2)]
        [InlineData(@"//%\n2%5\n3",3)]
        [InlineData(@"//%\n2%5\n3,5",4)]
        public void Parse_Returns_CorrectCount_For_SingleCharDelimiter(string paramString, int expectedCount)
        {
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Length);
        }

        [Theory]
        [InlineData(@"//[***]\n11***22***33",3)]
        [InlineData(@"//[::]\n11::22", 2)]
        [InlineData(@"//[%]\n2%5%3",3)]
        [InlineData(@"//[%%]\n2%%5%%3",3)]
        public void Parse_Returns_CorrectCount_For_MultiCharDelimiter(string paramString, int expectedCount)
        {
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Length);
        }
    }
}