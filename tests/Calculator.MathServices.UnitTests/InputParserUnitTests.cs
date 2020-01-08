using System;
using System.Collections.Generic;
using Xunit;
using Moq;

using Calculator.MathServices;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.UnitTests
{
    public class InputParserUnitTests
    {
        private readonly IInputParser _parser;
        private readonly Mock<IUserFlagParser> _userFlagParser;
        public InputParserUnitTests()
        {
            _userFlagParser = new Mock<IUserFlagParser>();
            _parser = new InputParser(_userFlagParser.Object);
        }

        [Theory]
        [InlineData("50",1)]
        [InlineData("50,3",2)]
        [InlineData(@"50\n3",2)]
        [InlineData(@"50\n3\n4\n1",4)]
        [InlineData("50,3,4",3)]
        public void Parse_Returns_CorrectCount_For_CommaAndNewLine(string paramString, int expectedCount)
        {
            SetUserFlagReturn(paramString);
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Values.Count);
        }

        [Theory]
        [InlineData(@"//#\n2#5#4",3)]
        [InlineData(@"//#\n2#5", 2)]
        [InlineData(@"//%\n2%5\n3",3)]
        [InlineData(@"//%\n2%5\n3,5",4)]
        public void Parse_Returns_CorrectCount_For_SingleCharDelimiter(string paramString, int expectedCount)
        {
            SetUserFlagReturn(paramString);
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Values.Count);
        }

        [Theory]
        [InlineData(@"//[***]\n11***22***33",3)]
        [InlineData(@"//[::]\n11::22", 2)]
        [InlineData(@"//[%]\n2%5%3",3)]
        [InlineData(@"//[%%]\n2%%5%%3",3)]
        public void Parse_Returns_CorrectCount_For_MultiCharDelimiter(string paramString, int expectedCount)
        {
            SetUserFlagReturn(paramString);
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Values.Count);
        }

        [Theory]
        [InlineData(@"//[*][!!][r9r]\n11r9r22*hh*33!!44",5)]
        [InlineData(@"//[::][?]\n11::22?3", 3)]
        [InlineData(@"//[%][--]\n2%5%3--4",4)]
        public void Parse_Returns_CorrectCount_For_Multiple_MultiCharDelimiter(string paramString, int expectedCount)
        {
            SetUserFlagReturn(paramString);
            var result = _parser.Parse(paramString);
            Assert.Equal(expectedCount, result.Values.Count);
        }

        private void SetUserFlagReturn(string userInput)
        {
            _userFlagParser.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns
                (
                    new UserInput
                    {
                        Delimiters = new List<string>(),
                        ParsedInput = userInput
                    }
                );
        }
    }
}