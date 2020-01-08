using System;
using System.Collections.Generic;
using Xunit;
using Moq;

using Calculator.MathServices;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.UnitTests
{
    public class MathServiceIntegrationTests
    {
        private readonly IMathService _mathService;
        private readonly IInputParser _inputParser;
        private readonly IUserFlagParser _userFlagParser;
        public MathServiceIntegrationTests()
        {
            _mathService = new MathService(new InputParser(new UserFlagParser()));
        }

        [Fact]
        public void Process_CorrectlyParses_SingleCharacter_CustomDelimiter()
        {
            string first = "//#\n10#";
            string second = "5#";
            string last = "3";

            var result = _mathService.Process($"{first}{second}{last}");

            Assert.Equal("18", GetTotal(result));  
        }

        [Theory]
        [InlineData("2,,4,rrrr,1001,6","2+0+4+0+0+6 = 12")]
        [InlineData("2,4,rrrr,1001,6","2+4+0+0+6 = 12")]
        [InlineData(@"//#\n2#5","2+5 = 7")]
        [InlineData(@"-n=t||//#\n2#-5","2+0 = 2")]
        [InlineData(@"-o=mul||//#\n2#5","2*5 = 10")]
        [InlineData(@"-o=Mul||//#\n2#5","2*5 = 10")]
        [InlineData(@"-o=div||3,3","3/3 = 1")]
        [InlineData(@"-o=sub||5,7","5-7 = -2")]
        [InlineData(@"-o=sub||10,7","10-7 = 3")]
        public void Process_Returns_Correct_Formula(string input, string expectedResult)
        {
            var result = _mathService.Process(input);

            Assert.Equal(expectedResult, result);  
        }

        private string GetTotal(string result)
        {
            return result.Substring(result.IndexOf("=") + 2);;
        }
    }
}