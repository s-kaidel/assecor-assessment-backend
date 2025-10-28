using Assecor.Backend.CsvAccess;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Mapping;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvPersonProvider(ILogger<CsvPersonProvider> logger, ICsvReader<CsvPerson> reader) : ICsvPersonProvider
    {
        public async Task<List<CsvPerson>> GetAllPersonsAsync()
        {
            var persons = await GetPersonsAsync();
            logger.LogInformation($"Found {persons.Count} person records");
            return persons;
        }

        public async Task<List<CsvPerson>> GetPersonsByColorAsync(Color color)
        {
            var persons = await GetPersonsAsync();

            var matchingPersons = persons
                .Where(x => x.Color != null && x.Color == color)
                .ToList();

            logger.LogInformation($"Found {matchingPersons.Count} persons for color '{color.ToString()}'");

            return matchingPersons;
        }

        public async Task<CsvPerson> GetPersonByIdAsync(int id)
        {
            var persons = await GetPersonsAsync();

            var person = persons.FirstOrDefault(x => x.Id == id);

            return person ?? throw new KeyNotFoundException($"No person matching id '{id}' found.");
        }

        private async Task<List<CsvPerson>> GetPersonsAsync()
        {
            return await reader.ReadFromCsvAsync(CsvPersonMapper.MapFromCsvRow);
        }
    }
}
