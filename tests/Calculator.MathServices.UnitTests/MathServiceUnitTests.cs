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
        [InlineData("-1","0",-1)]
        [InlineData("0","1",1)]
        [InlineData("1","3",4)]
        [InlineData("10","5",15)]
        public void Add_Returns_CorrectSum(string first, string second, int expectedResult)
        {
            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(result, expectedResult.ToString());
        }

        [Theory]
        [InlineData("-19","Welcome To The ThunderDome",-19)]
        [InlineData("50","RandomString",50)]
        [InlineData("30","",30)]
        [InlineData("12",null,12)]
        [InlineData("122","    ",122)]
        public void Add_Returns_Zero_When_InvalidParameters_ArePassed(string first, string second, int expectedResult)
        {
            var result = _mathService.Add($"{first},{second}");
            Assert.Equal(result, expectedResult.ToString());
        }

        [Fact]
        public void Add_ThrowsException_When_MoreThan_TwoValues_ArePassed()
        {
            Assert.Throws<ArgumentException>(() => _mathService.Add("1,2,3"));
            Assert.Throws<ArgumentException>(() => _mathService.Add("1,2,3,4"));
            Assert.Throws<ArgumentException>(() => _mathService.Add("1,2,3,4,5"));
        }
    }
}
