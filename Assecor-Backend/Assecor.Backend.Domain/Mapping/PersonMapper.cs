using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.DalModels;

namespace Assecor.Backend.Domain.Mapping
{
    public static class PersonMapper
    {
        public static List<Person> MapFromCsvPersons(IEnumerable<CsvPerson> csvPersons)
        {
            var persons = csvPersons.Select(MapFromCsvPerson).ToList();
            return persons;
        }

        public static Person MapFromCsvPerson(CsvPerson csvPerson)
        {
            var person = new Person()
            {
                Id = csvPerson.Id,
                Name = csvPerson.Name,
                LastName = csvPerson.LastName,
                City = csvPerson.City,
                Color = csvPerson.Color,
                ZipCode = csvPerson.ZipCode
            };
            return person;
        }
    }
}
