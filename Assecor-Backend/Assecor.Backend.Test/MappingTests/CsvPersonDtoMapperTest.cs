using Assecor.Backend.Domain.Requests;
using Assecor.Backend.Mappings;
using Assecor.Backend.Mappings.Interfaces;

namespace Assecor.Backend.Test.MappingTests
{
    public class CsvPersonDtoMapperTest
    {
        private readonly ICsvPersonDtoMapper _sut = new CsvPersonDtoMapper();

        [Theory]
        [InlineData(11111, "metropole", "11111 metropole")]
        [InlineData(11111, null, "11111")]
        [InlineData(11111, "", "11111")]
        [InlineData(11111, " ", "11111")]
        [InlineData(null, "metropole", "metropole")]
        [InlineData(null, "", null)]
        [InlineData(null, " ", null)]
        [InlineData(null, null, null)]
        public void Should_Create_Correct_Location_String(int? zipCode, string? city, string? result)
        {
            var request = new CreateCsvPersonRequest()
            {
                City = city,
                ZipCode = zipCode,
            };

            var dto = _sut.MapFromCreateCsvPersonRequest(request);

            dto.Location.ShouldBe(result);
        }
    }
}
