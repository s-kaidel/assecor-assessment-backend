using System.Globalization;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Dal.Provider
{
    public class CsvProvider(ILogger<CsvProvider> logger) : ICsvProvider
    {
        public List<CsvPerson> ReadPersonsFromCsv()
        {
            try
            {
                using var reader = new StreamReader("data.csv");
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
