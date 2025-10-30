using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;
using Assecor.Backend.Mappings;
using Assecor.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class PersonService(ICsvPersonProvider csvProvider) : IPersonService
    {
        private readonly ICsvPersonProvider _csvProvider = csvProvider;

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var dalPersons = await _csvProvider.GetAllPersonsAsync();
            var persons = PersonMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }

        public async Task<List<Person>> GetPersonsByColorAsync(Color color)
        {
            var csvPersons = await _csvProvider.GetPersonsByColorAsync(color);
            var persons = PersonMapper.MapFromCsvPersons(csvPersons);
            return persons;
        }

        public async Task<Maybe<Person>> GetPersonByIdAsync(int id)
        {
            var csvPerson = await _csvProvider.GetPersonByIdAsync(id);
            var person = csvPerson.Map(PersonMapper.MapFromCsvPerson);
            return person;
        }
    }
}
