using Assecor.Backend.Configuration;
using Assecor.Backend.Domain.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Assecor.Backend.CsvAccess
{
    public class CsvReader<T>(ILogger<CsvReader<T>> logger, IOptions<CsvOptions> options) : ICsvReader<T>
    {
        private readonly string _filePath = options.Value.CsvFilePath;
        public async Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc)
        {
            CheckFilePath();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // ignore missing headers
                MissingFieldFound = null // ignore missing fields
            };

            logger.LogInformation($"Reading csv file from location: {_filePath}");
            var items = new List<T>();
            var rowNumber = 1;

            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, config);

                while (await csv.ReadAsync())
                {
                    
                    var fields = csv.Parser.Record ?? [];
                    var item = mappingFunc.Invoke(fields, rowNumber);
                    if (item != null)
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

            return items;
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
