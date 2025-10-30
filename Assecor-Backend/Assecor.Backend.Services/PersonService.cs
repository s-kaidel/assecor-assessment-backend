using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;
using Assecor.Backend.Mappings;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class PersonService(ICsvPersonProvider csvProvider, IPersonMapper personMapper) : IPersonService
    {
        private readonly ICsvPersonProvider _csvProvider = csvProvider;
        private readonly IPersonMapper _personMapper = personMapper;

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var dalPersons = await _csvProvider.GetAllPersonsAsync();
            var persons = _personMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }

        public async Task<List<Person>> GetPersonsByColorAsync(Color color)
        {
            var csvPersons = await _csvProvider.GetPersonsByColorAsync(color);
            var persons = _personMapper.MapFromCsvPersons(csvPersons);
            return persons;
        }

        public async Task<Maybe<Person>> GetPersonByIdAsync(int id)
        {
            var csvPerson = await _csvProvider.GetPersonByIdAsync(id);
            var person = csvPerson.Map(_personMapper.MapFromCsvPerson);
            return person;
        }
    }
}
