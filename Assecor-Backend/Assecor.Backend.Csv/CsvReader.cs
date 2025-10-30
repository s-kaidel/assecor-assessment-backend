using Assecor.Backend.Domain.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Assecor.Backend.CsvAccess
{
    public class CsvReader<T>(ILogger<CsvReader<T>> logger) : ICsvReader<T>
    {
        private readonly ILogger<CsvReader<T>> _logger = logger;

        public async Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc, string filePath)
        {
            
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // ignore missing headers
                MissingFieldFound = null // ignore missing fields
            };

            _logger.LogInformation("Beginning csv parsing.");
            var items = new List<T>();
            var rowNumber = 1;

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, config);

                while (await csv.ReadAsync())
                {
                    var fields = csv.Parser.Record ?? [];
                    var item = mappingFunc.Invoke(fields, rowNumber);

                    if (item == null)
                    {
                        _logger.LogInformation($"Row {rowNumber} could not be parsed, row is skipped.");
                    }
                    else
                    {
                        items.Add(item);
                    }
                    rowNumber++;
                }
            }
            catch (Exception ex)
            {
                throw new CsvReaderException($"Csv reading error in row {rowNumber}: {ex.Message}");
            }

            _logger.LogInformation("Csv parsing successful");
            return items;
        }
    }
}
