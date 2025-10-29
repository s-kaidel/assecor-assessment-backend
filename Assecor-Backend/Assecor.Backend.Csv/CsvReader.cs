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
        private readonly ILogger<CsvReader<T>> _logger = logger;
        private readonly string _fileName = options.Value.FileName;

        public async Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc)
        {
            CheckFilePath();
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
                using var reader = new StreamReader(GetFilePath());
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
        private void CheckFilePath()
        {
            if (File.Exists(GetFilePath()))
            {
                return;
            }
            throw new FileNotFoundException("Csv file not found, please review file name in appSettings");
        }

        private string GetFilePath()
        {
            return Path.Combine(AppContext.BaseDirectory, _fileName);
        }
    }
}
