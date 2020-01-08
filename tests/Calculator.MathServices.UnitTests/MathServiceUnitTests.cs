using System;
using System.Collections.Generic;
using Xunit;
using Moq;

using Calculator.MathServices;
using Calculator.MathServices.Helpers;
using Calculator.MathServices.Models;

namespace Calculator.MathServices.UnitTests
{
    public class MathServiceUnitTests
    {
        private readonly IMathService _mathService;
        private readonly Mock<IInputParser> _parser;
        private readonly string _customDelimiter = "//\n";
        
        public MathServiceUnitTests()
        {
            _parser = new Mock<IInputParser>();
            _mathService = new MathService(_parser.Object);
        }

        [Theory]
        [InlineData("0","1",1)]
        [InlineData("1","3",4)]
        [InlineData("10","5",15)]
        public void Process_Returns_CorrectSum(string first, string second, int expectedResult)
        {
            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second));

            var result = _mathService.Process($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), GetTotal(result));
        }

        [Theory]
        [InlineData("50","RandomString",50)]
        [InlineData("30","",30)]
        [InlineData("12",null,12)]
        [InlineData("122","    ",122)]
        [InlineData("1002","2",2)]
        [InlineData("3003","6",6)]
        public void Process_Returns_Zero_When_InvalidParameters_ArePassed(string first, string second, int expectedResult)
        {
            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second));
            
            var result = _mathService.Process($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), GetTotal(result));
        }

        [Fact]
        public void Process_Accepts_MultipleParameters()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));

            var result = _mathService.Process($"{first},{second},{last}");

            Assert.Equal("10", GetTotal(result));
        }

        [Fact]
        public void Process_Accepts_NewLine_AsDelimiter()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));

            var result = _mathService.Process($"{first}{@"\n"}{second},{last}");

            Assert.Equal("10", GetTotal(result));
        }

        [Fact] 
        public void Process_ThrowsException_NegativeValues_ArePassed() 
        { 
            string first = "-2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));
            
            Assert.Throws<ArgumentException>(() => _mathService.Process($"{first}{@"\n"}{second},{last}")); 
        } 

        [Fact]
        public void Process_Returns_Zero_When_InvalidParameters_ArePassed_And_MoreThan_TwoParameters()
        {
            string first = $"{_customDelimiter}10000";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));
                    
            var result = _mathService.Process($"{first},{second},{last}");

            Assert.Equal("8", GetTotal(result));  
        }

        private string GetTotal(string result)
        {
            return result.Substring(result.IndexOf("=") + 2);;
        }

        private UserInput CreateParserReturn(string first, string second, string last = "")
        {
            return new UserInput() { Values = new List<string>(){$"{first}", $"{second}", $"{last}"}};
        }
    }
}
