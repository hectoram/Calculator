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
        public void Add_Returns_CorrectSum(string first, string second, int expectedResult)
        {
            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second));

            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), GetTotal(result));
        }

        [Theory]
        [InlineData("50","RandomString",50)]
        [InlineData("30","",30)]
        [InlineData("12",null,12)]
        [InlineData("122","    ",122)]
        [InlineData("1002","2",2)]
        [InlineData("3003","6",6)]
        public void Add_Returns_Zero_When_InvalidParameters_ArePassed(string first, string second, int expectedResult)
        {
            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second));
            
            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), GetTotal(result));
        }

        [Fact]
        public void Add_Accepts_MultipleParameters()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));

            var result = _mathService.Add($"{first},{second},{last}");

            Assert.Equal("10", GetTotal(result));
        }

        [Fact]
        public void Add_Accepts_NewLine_AsDelimiter()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));

            var result = _mathService.Add($"{first}{@"\n"}{second},{last}");

            Assert.Equal("10", GetTotal(result));
        }

        [Fact] 
        public void Add_ThrowsException_NegativeValues_ArePassed() 
        { 
            string first = "-2";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));
            
            Assert.Throws<ArgumentException>(() => _mathService.Add($"{first}{@"\n"}{second},{last}")); 
        } 

        [Fact]
        public void Add_Returns_Zero_When_InvalidParameters_ArePassed_And_MoreThan_TwoParameters()
        {
            string first = $"{_customDelimiter}10000";
            string second = "5";
            string last = "3";

            _parser.Setup(x => x.Parse(It.IsAny<string>()))
                    .Returns(CreateParserReturn(first, second, last));
                    
            var result = _mathService.Add($"{first},{second},{last}");

            Assert.Equal("8", GetTotal(result));  
        }

        [Fact]
        public void Add_CorrectlyParses_SingleCharacter_CustomDelimiter()
        {
            string first = "//#\n10#";
            string second = "5#";
            string last = "3";

            var temp = new MathService(new InputParser(new UserFlagParser()));

            var result = temp.Add($"{first}{second}{last}");

            Assert.Equal("18", GetTotal(result));  
        }

        [Theory]
        [InlineData("2,,4,rrrr,1001,6","2+0+4+0+0+6 = 12")]
        [InlineData("2,4,rrrr,1001,6","2+4+0+0+6 = 12")]
        [InlineData(@"//#\n2#5","2+5 = 7")]
        [InlineData(@"-n=t||//#\n2#-5","2+0 = 2")]
        public void Add_Returns_Correct_AdditonFormula(string input, string expectedResult)
        {
            var temp = new MathService(new InputParser(new UserFlagParser()));
            var result = temp.Add(input);

            Assert.Equal(expectedResult, result);  
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
