using System;
using Xunit;

using Calculator.MathServices;

namespace Calculator.MathServices.UnitTests
{
    public class MathServiceUnitTests
    {
         private readonly MathService _mathService;

        public MathServiceUnitTests()
        {
            _mathService = new MathService();
        }

        [Theory]
        [InlineData("0","1",1)]
        [InlineData("1","3",4)]
        [InlineData("10","5",15)]
        public void Add_Returns_CorrectSum(string first, string second, int expectedResult)
        {
            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), result);
        }

        [Theory]
        [InlineData("50","RandomString",50)]
        [InlineData("30","",30)]
        [InlineData("12",null,12)]
        [InlineData("122","    ",122)]
        public void Add_Returns_Zero_When_InvalidParameters_ArePassed(string first, string second, int expectedResult)
        {
            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(expectedResult.ToString(), result);
        }

        [Fact]
        public void Add_Accepts_MultipleParameters()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            var result = _mathService.Add($"{first},{second},{last}");

            Assert.Equal("10", result);
        }

        [Fact]
        public void Add_Accepts_NewLine_AsDelimiter()
        {
            string first = "2";
            string second = "5";
            string last = "3";

            var result = _mathService.Add($"{first}{@"\n"}{second},{last}");

            Assert.Equal("10", result);
        }

        [Fact] 
        public void Add_ThrowsException_NegativeValues_ArePassed() 
        { 
            Assert.Throws<ArgumentException>(() => _mathService.Add("1,-2,3")); 
            Assert.Throws<ArgumentException>(() => _mathService.Add("1,2,3,-4")); 
            Assert.Throws<ArgumentException>(() => _mathService.Add("-1,4,5")); 
        } 
    }
}
