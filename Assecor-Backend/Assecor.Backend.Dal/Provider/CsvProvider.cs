using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Dal.Helper;
using Assecor.Backend.Domain.DalModels;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvProvider(ILogger<CsvProvider> logger, IOptions<CsvOptions> options) : ICsvProvider
    {
        private readonly string _filePath = options.Value.CsvFilePath;
        private readonly CsvReaderHelper _helper = new();
        public async Task<List<CsvPerson>> ReadPersonsFromCsvAsync()
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null, // ignore missing headers
                    MissingFieldFound = null // ignore missing fields
                };
                logger.LogInformation($"Reading csv file from location: {_filePath}");

                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, config);
                var persons = await ReadPersons(csv);

                logger.LogInformation($"Found {persons.Count} person records");
                return persons;
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred while trying to read persons from csv: {e.Message}");
                return new();
            }
        }

        private async Task<List<CsvPerson>> ReadPersons(CsvReader csv)
        {
            var persons = new List<CsvPerson>();
            var rowNumber = 1;
            while (await csv.ReadAsync())
            {
                var fields = csv.Parser.Record;
                var person = _helper.GetPersonFromCsvRow(fields ?? [], rowNumber);
                persons.Add(person);
                rowNumber++;
            }

            return persons;
        }
    }
}
