using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Services;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Test.ServiceTests
{
    public class ValidationServiceTest
    {
        private readonly IValidationService _sut = new ValidationService();

        [Theory]
        [InlineData(1, true)]
        [InlineData(-1, false)]
        [InlineData(int.MaxValue, false)]
        public void IsValidEnum_Should_Return_Correct_Value(int value, bool expectedOutcome)
        {
            _sut.IsValidEnumValue<Color>(value).ShouldBe(expectedOutcome);
        }
    }
}
