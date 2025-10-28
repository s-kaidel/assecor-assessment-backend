using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Dal.Helper;
using Assecor.Backend.Domain.DalModels;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Exceptions;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvPersonProvider(ILogger<CsvPersonProvider> logger, IOptions<CsvOptions> options) : ICsvPersonProvider
    {
        private readonly string _filePath = options.Value.CsvFilePath;
        private readonly CsvReaderHelper _helper = new();
        public async Task<List<CsvPerson>> GetAllPersonsAsync()
        {
            var persons = await ReadPersonsFromCsvAsync();

            logger.LogInformation($"Found {persons.Count} person records");
            return persons;
        }

        public async Task<List<CsvPerson>> GetPersonsByColorAsync(Color color)
        {
            var persons = await ReadPersonsFromCsvAsync();

            var matchingPersons = persons.Where(x => x.Color != null && x.Color == color).ToList();

            logger.LogInformation($"Found {matchingPersons.Count} persons for color {color.ToString()}");

            return matchingPersons;
        }

        public async Task<CsvPerson?> GetPersonByIdAsync(int id)
        {
            var persons = await ReadPersonsFromCsvAsync();

            var person = persons.FirstOrDefault(x => x.Id == id);

            return person ?? throw new KeyNotFoundException($"No person matching id {id} found.");
        }

        private async Task<List<CsvPerson>> ReadPersonsFromCsvAsync()
        {
            CheckFilePath();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // ignore missing headers
                MissingFieldFound = null // ignore missing fields
            };

            logger.LogInformation($"Reading csv file from location: {_filePath}");
            var persons = new List<CsvPerson>();
            var rowNumber = 1;

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, config);

                while (await csv.ReadAsync())
                {
                    
                    var fields = csv.Parser.Record;
                    var person = _helper.GetPersonFromCsvRow(fields ?? [], rowNumber);
                    if (person != null)
                    {
                        persons.Add(person);
                    }
                    rowNumber++;
                }
            }
            catch (Exception ex)
            {
                throw new CsvReaderException($"Csv reading error in row {rowNumber}: {ex.Message}");
            }

            return persons;
        }
        private void CheckFilePath()
        {
            if (File.Exists(_filePath))
            {
                return;
            }
            throw new FileNotFoundException("Csv file not found, please review file location in appSettings");
        }
    }
}
