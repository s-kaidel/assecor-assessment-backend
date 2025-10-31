using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;
using Assecor.Backend.Domain.Requests;
using Assecor.Backend.Mappings.Interfaces;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class PersonService(ICsvPersonProvider csvProvider, IPersonMapper personMapper, ICsvPersonDtoMapper dtoMapper) : IPersonService
    {
        private readonly ICsvPersonProvider _csvProvider = csvProvider;
        private readonly IPersonMapper _personMapper = personMapper;
        private readonly ICsvPersonDtoMapper _dtoMapper = dtoMapper;
        /// <summary>
        /// Returns all currently available persons from the data storage
        /// </summary>
        /// <returns></returns>
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var dalPersons = await _csvProvider.GetAllPersonsAsync();
            var persons = _personMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }

        /// <summary>
        /// Returns all persons whose favorite color matches provided color
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        public async Task<List<Person>> GetPersonsByColorAsync(Color color)
        {
            var csvPersons = await _csvProvider.GetPersonsByColorAsync(color);
            var persons = _personMapper.MapFromCsvPersons(csvPersons);
            return persons;
        }

        /// <summary>
        /// Returns the person matching provided id. If none found, an empty maybe is returned
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        public async Task<Maybe<Person>> GetPersonByIdAsync(int id)
        {
            var csvPerson = await _csvProvider.GetPersonByIdAsync(id);
            var person = csvPerson.Map(_personMapper.MapFromCsvPerson);
            return person;
        }

        //TODO add unit test
        /// <summary>
        /// Create a new person entry in the data storage, returns id of created person
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> CreateNewCsvPersonAsync(CreateCsvPersonRequest request)
        {
            var dto = _dtoMapper.MapFromCreateCsvPersonRequest(request);
            var id = await _csvProvider.CreateCsvPersonAsync(dto);
            return id;
        }
    }
}
