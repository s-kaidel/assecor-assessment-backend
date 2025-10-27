using System.Globalization;
using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvProvider(ILogger<CsvProvider> logger, IOptions<CsvOptions> options) : ICsvProvider
    {
        private readonly string _filePath = options.Value.CsvFilePath;
        public List<CsvPerson> ReadPersonsFromCsv()
        {
            try
            {
                logger.LogInformation($"Reading csv file from location: {_filePath}");
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var persons = csv.GetRecords<CsvPerson>().ToList();
                return persons;
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred while trying to read persons from csv: {e.Message}");
                return new();
            }
        }
    }
}
