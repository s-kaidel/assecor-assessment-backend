using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Mappings;
using Assecor.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Test.MappingTests
{
    public class CsvPersonMapperTest
    {
        private readonly ICsvPersonMapper _sut;
        private readonly ILogger<CsvPersonMapper> _loggerMock = Substitute.For<ILogger<CsvPersonMapper>>();
        private readonly IValidationService _validationMock = Substitute.For<IValidationService>();

        public CsvPersonMapperTest()
        {
            _sut = new CsvPersonMapper(_loggerMock, _validationMock);
        }

        private void SetupValidationMock(bool isValid = true) =>
            _validationMock.IsValidEnumValue<Color>(Arg.Any<int>()).Returns(isValid);
        
        private static List<string> GetFieldsList(string name, string? lastName, string? location, string? color)
        {
            List<string> fields = [name];
            AddString(fields, lastName);
            AddString(fields, location);
            AddString(fields, color?.ToString());
            return fields;
        }

        private static void AddString(List<string> list, string? str)
        {
            if (str != null)
            {
                list.Add(str);
            }
        }

        [Fact]
        public void Should_Return_Null_On_Empty_Fields()
        {
            var person = _sut.MapFromCsvRow([], 1);
            person.ShouldBeNull();
        }

        public static TheoryData<string, string?, string?, string?, int?, string?, Color?> MappingData => new()
        {
            //complete mapping
            {"name", "nachname", "11111 metropole", "1", 11111, "metropole", Color.Blau},

            //color is null or not parsable to int
            {"name", "nachname", "11111 metropole", null, 11111, "metropole", null},
            {"name", "nachname", "11111 metropole", "abc", 11111, "metropole", null},

            //location is missing
            {"name", "nachname", null, "1", null, null, Color.Blau},

            //location has no zipCode
            {"name", "nachname", "metropole", "1", null, null, Color.Blau},

            //location has faulty zipCode
            {"name", "nachname", "1abcd metropole", "1", null, "metropole", Color.Blau},

            //missing lastName
            {"name", null, "11111 metropole", "1", 11111, "metropole", Color.Blau},

            //name only
            {"name", null, null, null, null, null, null},

            //name nad lastName only
            {"name", "lastName", null, null, null, null, null},
        };

        [Theory]
        [MemberData(nameof(MappingData))]
        public void Should_Map_Correct_Person(string name, string? lastName, string? location, string? color, int? expectedZipCode, string? expectedCity, Color? expectedColor)
        {
            SetupValidationMock();
            var rowNumber = 1;
            var fields = GetFieldsList(name, lastName, location, color);

            var person = _sut.MapFromCsvRow(fields, rowNumber);

            person.ShouldNotBeNull();
            person.Id.ShouldBe(rowNumber);
            person.Name.ShouldBe(name);
            person.LastName.ShouldBe(lastName);
            person.ZipCode.ShouldBe(expectedZipCode);
            person.City.ShouldBe(expectedCity);
            person.Color.ShouldBe(expectedColor);
        }

        [Fact]
        public void Color_Should_Be_Null_If_Not_In_Enum()
        {
            SetupValidationMock(false);
            string[] fields = ["name", "nachname", "11111 metropole", "1000"];

            var person = _sut.MapFromCsvRow(fields, 0);
            person.ShouldNotBeNull();
            person.Color.ShouldBeNull();
        }
    }
}
