using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Mappings;
using Assecor.Backend.Mappings.Interfaces;

namespace Assecor.Backend.Test.MappingTests
{
    public class PersonMapperTest
    {
        private readonly IPersonMapper _sut = new PersonMapper();
        [Fact]
        public void Should_Map_Correct_Person()
        {
            var csvPerson = new CsvPerson()
            {
                Id = 1,
                Name = "Hans",
                LastName = "Habicht",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Rot
            };

            var person = _sut.MapFromCsvPerson(csvPerson);

            person.Id.ShouldBe(csvPerson.Id);
            person.Name.ShouldBe(csvPerson.Name);
            person.LastName.ShouldBe(csvPerson.LastName);
            person.City.ShouldBe(csvPerson.City);
            person.ZipCode.ShouldBe(csvPerson.ZipCode);
            person.Color.ShouldBe(csvPerson.Color);
        }

        [Fact]
        public void Should_Map_All_Persons()
        {
            var csvPerson1 = new CsvPerson()
            {
                Id = 1,
                Name = "Hans",
                LastName = "Habicht",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Rot
            };

            var csvPerson2 = new CsvPerson()
            {
                Id = 2,
                Name = "Gundula",
                LastName = "Geier",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Blau
            };

            var expectedCount = 2;

            var persons = _sut.MapFromCsvPersons([csvPerson1, csvPerson2]);
            persons.Count.ShouldBe(expectedCount);
        }
    }
}
