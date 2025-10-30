using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Mapping;

namespace Assecor.Backend.Test.MappingTests
{
    public class CsvPersonMapperTest
    {
        [Fact]
        public void Should_Return_Null_On_Empty_Fields()
        {
            var person = CsvPersonMapper.MapFromCsvRow([], 1);
            person.ShouldBeNull();
        }

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

        public static TheoryData<string, string?, string?, string?, int?, string?, Color?> MappingData => new()
        {
            //complete mapping
            {"name", "nachname", "11111 metropole", "1", 11111, "metropole", Color.Blau},

            //color is null or not in enum or not parsable to int
            {"name", "nachname", "11111 metropole", null, 11111, "metropole", null},
            {"name", "nachname", "11111 metropole", "1000", 11111, "metropole", null},
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
            
            var rowNumber = 1;
            var fields = GetFieldsList(name, lastName, location, color);

            var person = CsvPersonMapper.MapFromCsvRow(fields, rowNumber);

            person.ShouldNotBeNull();
            person.Id.ShouldBe(rowNumber);
            person.Name.ShouldBe(name);
            person.LastName.ShouldBe(lastName);
            person.ZipCode.ShouldBe(expectedZipCode);
            person.City.ShouldBe(expectedCity);
            person.Color.ShouldBe(expectedColor);
        }
    }
}
