using System;
using System.Collections.Generic;
using Xunit;
using Moq;

using Calculator.MathServices;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.UnitTests
{
    public class UserInputFlagParserUnitTests
    {
        private readonly IUserFlagParser _userFlagParser;
        public UserInputFlagParserUnitTests()
        {
            _userFlagParser = new UserFlagParser();
        }

        [Theory]
        [InlineData("-d=^||1,2^3^4\n^3", "^")]
        [InlineData("-b=1005-d=$||1,2^3^4\n^3", "$")]
        [InlineData("-b=1005-n=f-d=*||1,2^3^4\n^3", "*")]
        public void Parse_ReturnsCorrect_Delimiter(string paramString, string delimiterLookup)
        {
            var result = _userFlagParser.Parse(paramString);
            Assert.True(result.Delimiters.Contains(delimiterLookup));
        }

        [Theory]
        [InlineData("-n=f||1,2^3^4\n^3", false)]
        [InlineData("-n=t||1,2^3^4\n^3", true)]
        [InlineData("-b=1005-n=0||1,2^3^4\n^3", false)]
        [InlineData("-b=1005-n=1-d=^||1,2^3^4\n^3", true)]
        [InlineData("-b=1005-n=garbage-d=^||1,2^3^4\n^3", false)]
        public void Parse_ReturnsCorrect_NegativeAllowance(string paramString, bool expectedNegativeAllowance)
        {
            var result = _userFlagParser.Parse(paramString);
            Assert.Equal(expectedNegativeAllowance, result.AllowNegatives);
        }

        [Theory]
        [InlineData("-b=1005-n=0||1,2^3^4\n^3", 1005)]
        [InlineData("-n=1-b=3007-d=^||1,2^3^4\n^3", 3007)]
        [InlineData("n=garbage-d=^-b=105||1,2^3^4\n^3", 105)]
        public void Parse_ReturnsCorrect_UpperBound(string paramString, int expectedUpperBound)
        {
            var result = _userFlagParser.Parse(paramString);
            Assert.Equal(expectedUpperBound, result.UpperBound);
        }
    }
}