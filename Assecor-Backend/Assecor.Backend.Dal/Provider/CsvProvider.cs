using System.Globalization;
using Assecor.Backend.Configuration;
using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using CsvHelper;
using CsvHelper.Configuration;
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
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null, // fehlende Header ignorieren
                    MissingFieldFound = null // fehlende Felder ignorieren
                };
                logger.LogInformation($"Reading csv file from location: {_filePath}");
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, config);
                var persons = ReadPersons(csv);
                return persons;
            }
            catch (Exception e)
            {
                logger.LogError($"An error occurred while trying to read persons from csv: {e.Message}");
                return new();
            }
        }

        private List<CsvPerson> ReadPersons(CsvReader csv)
        {
            var persons = new List<CsvPerson>();
            var rowNumber = 1;
            while (csv.Read())
            {
                var id = rowNumber;
                var name = csv.GetField(0)?.Trim();
                var lastName = csv.GetField(1)?.Trim();
                var location = csv.GetField(2);
                var color = int.TryParse(csv.GetField(3), out var colorNr);

                var split = location?.Trim().Split(' ');
                var zipCode = int.TryParse(split?[0], out var zip);
                var city = split?[1];

                var person = new CsvPerson()
                {
                    Id = id,
                    Name = name ?? string.Empty,
                    LastName = lastName ?? string.Empty,
                    Color = (Color)colorNr,
                    City = city ?? string.Empty,
                    ZipCode = zip,
                };

                persons.Add(person);
                rowNumber++;
            }

            return persons;
        }
    }
}
