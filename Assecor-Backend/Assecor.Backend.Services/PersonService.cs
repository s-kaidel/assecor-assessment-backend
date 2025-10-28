using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Mapping;
using Assecor.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class PersonService(ICsvPersonProvider csvProvider) : IPersonService
    {
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var dalPersons = await csvProvider.GetAllPersonsAsync();
            var persons = PersonMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }

        public async Task<List<Person>> GetPersonsByColorAsync(Color color)
        {
            var csvPersons = await csvProvider.GetPersonsByColorAsync(color);
            var persons = PersonMapper.MapFromCsvPersons(csvPersons);
            return persons;
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            Person? person = null;
            var csvPerson = await csvProvider.GetPersonByIdAsync(id);

            if (csvPerson != null)
            {
                person = PersonMapper.MapFromCsvPerson(csvPerson);
            }
            return person;
        }
    }
}
