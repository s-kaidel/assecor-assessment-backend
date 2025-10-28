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

            if (person is null)
            {
                var error = $"No person matching id {id} found.";
                logger.LogInformation(error);
                throw new KeyNotFoundException(error);
            }

            return person;
        }

        private void CheckFilePath()
        {
            if (File.Exists(_filePath))
            {
                return;
            }
            var error = "Csv file not found, please review provided file location in appSettings";
            throw new FileNotFoundException(error);
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

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, config);

            while (await csv.ReadAsync())
            {
                try
                {
                    var fields = csv.Parser.Record;
                    var person = _helper.GetPersonFromCsvRow(fields ?? [], rowNumber);
                    if (person != null)
                    {
                        persons.Add(person);
                    }

                    rowNumber++;
                }
                catch (Exception ex)
                {
                    var error = $"Csv reading error in row {rowNumber}: {ex}";
                    throw new CsvReaderException(error);
                }
            }

            return persons;
        }
    }
}
