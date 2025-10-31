using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Requests;
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

        [Theory]
        [InlineData("hans", "herrmann", 1, true)]
        [InlineData("hans", "herrmann", -1, false)]
        [InlineData("hans", "herrmann", int.MaxValue, false)]
        [InlineData("", "herrmann", 1, false)]
        [InlineData("hans", "", 1, false)]
        public void Should_Validate_PersonRequest(string name, string lastName, int? color, bool expectedOutcome)
        {
            var request = new CreateCsvPersonRequest()
            {
                Name = name,
                LastName = lastName,
                Color = color,
            };

            _sut.IsValidCsvPerson(request).ShouldBe(expectedOutcome);
        }
    }
}
