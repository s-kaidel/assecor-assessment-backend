using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Mapping;

namespace Assecor.Backend.Test.MappingTests
{
    public class ApiPersonMapperTest
    {
        [Fact]
        public void Should_Map_Correct_Person()
        {
            var person = new Person()
            {
                Id = 1,
                Name = "Hans",
                LastName = "Habicht",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Rot
            };

            var apiPerson = ApiPersonMapper.MapFromDomainPerson(person);

            var expectedColor = person.Color.ToString()?.ToLower();

            apiPerson.Id.ShouldBe(person.Id);
            apiPerson.Name.ShouldBe(person.Name);
            apiPerson.LastName.ShouldBe(person.LastName);
            apiPerson.City.ShouldBe(person.City);
            apiPerson.ZipCode.ShouldBe(person.ZipCode);
            apiPerson.Color.ShouldBe(expectedColor);
        }

        [Fact]
        public void Should_Map_All_Persons()
        {
            var person1 = new Person()
            {
                Id = 1,
                Name = "Hans",
                LastName = "Habicht",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Rot
            };

            var person2 = new Person()
            {
                Id = 2,
                Name = "Gundula",
                LastName = "Geier",
                City = "Falkendorf",
                ZipCode = 91074,
                Color = Color.Blau
            };

            var expectedCount = 2;

            var persons = ApiPersonMapper.MapFromDomainPersons([person1, person2]);
            persons.Count.ShouldBe(expectedCount);
        }
    }
}
