using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Extensions;
using Assecor.Backend.Domain.Maybe;
using Assecor.Backend.Mappings.Interfaces;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvPersonProvider(
        ILogger<CsvPersonProvider> logger,
        ICsvReader<CsvPerson> reader,
        ICsvFileLocationHandler fileLocationHandler,
        ICsvPersonMapper mapper,
        ICsvWriter<CsvPersonDto> writer) : ICsvPersonProvider
    {
        private readonly ILogger<CsvPersonProvider> _logger = logger;
        private readonly ICsvReader<CsvPerson> _reader = reader;
        private readonly ICsvFileLocationHandler _fileLocationHandler = fileLocationHandler;
        private readonly ICsvPersonMapper _mapper = mapper;
        private readonly ICsvWriter<CsvPersonDto> _writer = writer;

        /// <summary>
        /// Returns all currently available persons from the csv data file.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CsvPerson>> GetAllPersonsAsync()
        {
            var persons = await GetPersonsAsync();
            _logger.LogInformation("Found {Count} person", persons.Count);
            return persons;
        }

        /// <summary>
        /// Returns all persons whose favorite color matches provided color.
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        public async Task<List<CsvPerson>> GetPersonsByColorAsync(Color color)
        {
            var persons = await GetPersonsAsync();

            var matchingPersons = persons
                .Where(x => x.Color != null && x.Color == color)
                .ToList();

            _logger.LogInformation("Found {MatchingPersonsCount} persons for color '{Color}'", matchingPersons.Count, color.ToString());

            return matchingPersons;
        }

        /// <summary>
        /// Returns the person matching provided id.
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        public async Task<Maybe<CsvPerson>> GetPersonByIdAsync(int id)
        {
            var persons = await GetPersonsAsync();

            var person = persons.FirstOrDefault(x => x.Id == id);

            var personMaybe = Maybe<CsvPerson>.From(person);

            _logger.LogMaybeResult(personMaybe, id);

            return personMaybe;
        }

        /// <summary>
        /// Create a person entry in the csv data file. Returns the id of created person.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> CreateCsvPersonAsync(CsvPersonDto dto)
        {
            var filePath = GetPersonsFilePath();
            var id = await writer.AppendToCsvAsync(filePath, [dto]);
            _logger.LogInformation("Successfully created new person with id '{id}'", id);
            return id;
        }

        private async Task<List<CsvPerson>> GetPersonsAsync()
        {
            var filePath = GetPersonsFilePath();
            return await _reader.ReadFromCsvAsync(_mapper.MapFromCsvRow, filePath);
        }

        private string GetPersonsFilePath()
        {
            return _fileLocationHandler.GetPersonsFilePath();
        }
    }
}
