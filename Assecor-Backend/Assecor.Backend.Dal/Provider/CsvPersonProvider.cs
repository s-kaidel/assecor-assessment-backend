using Assecor.Backend.Configuration;
using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Extensions;
using Assecor.Backend.Domain.Mapping;
using Assecor.Backend.Domain.Maybe;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvPersonProvider(ILogger<CsvPersonProvider> logger, ICsvReader<CsvPerson> reader, ICsvFileLocationHandler fileLocationHandler, IOptions<CsvSettings> settings) : ICsvPersonProvider
    {
        private readonly ILogger<CsvPersonProvider> _logger = logger;
        private readonly ICsvReader<CsvPerson> _reader = reader;
        private readonly ICsvFileLocationHandler _fileLocationHandler = fileLocationHandler;
        private readonly CsvSettings _csvSettings = settings.Value;

        public async Task<List<CsvPerson>> GetAllPersonsAsync()
        {
            var persons = await GetPersonsAsync();
            _logger.LogInformation("Found {Count} person", persons.Count);
            return persons;
        }

        public async Task<List<CsvPerson>> GetPersonsByColorAsync(Color color)
        {
            var persons = await GetPersonsAsync();

            var matchingPersons = persons
                .Where(x => x.Color != null && x.Color == color)
                .ToList();

            _logger.LogInformation("Found {MatchingPersonsCount} persons for color '{Color}'", matchingPersons.Count, color.ToString());

            return matchingPersons;
        }

        public async Task<Maybe<CsvPerson>> GetPersonByIdAsync(int id)
        {
            var persons = await GetPersonsAsync();

            var person = persons.FirstOrDefault(x => x.Id == id);

            var personMaybe = Maybe<CsvPerson>.From(person);

            _logger.LogMaybeResult(personMaybe, id);

            return personMaybe;
        }

        private async Task<List<CsvPerson>> GetPersonsAsync()
        {
            var filePath = _fileLocationHandler.GetCsvFilePath(_csvSettings.Files.Persons);
            return await _reader.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow, filePath);
        }
    }
}
