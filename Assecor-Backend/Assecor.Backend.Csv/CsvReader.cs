using Assecor.Backend.Domain.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Assecor.Backend.CsvAccess.Interfaces;

namespace Assecor.Backend.CsvAccess
{
    public class CsvReader<T>(ILogger<CsvReader<T>> logger) : ICsvReader<T>
    {
        private readonly ILogger<CsvReader<T>> _logger = logger;

        /// <summary>
        /// Tries to parse a csv file with provided mapping method
        /// </summary>
        /// <param name="mappingFunc">the mapping method to use while parsing</param>
        /// <param name="filePath">the full filePath</param>
        /// <returns></returns>
        /// <exception cref="CsvReaderException"></exception>
        public async Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc, string filePath)
        {
            
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // ignore missing headers
                MissingFieldFound = null // ignore missing fields
            };

            _logger.LogInformation("Beginning csv parsing.");
            var items = new List<T>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            var rowNumber = 0;

            try
            {
                while (await csv.ReadAsync())
                {
                    rowNumber = csv.Parser.Row;
                    var fields = csv.Parser.Record ?? [];
                    var item = mappingFunc.Invoke(fields, rowNumber);

                    if (item == null)
                    {
                        _logger.LogInformation("Row {row} could not be parsed to object of type {typeName}, row is skipped.", rowNumber, typeof(T).Name);
                        
                    }
                    else
                    {
                        items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CsvReaderException($"Csv reading error in row {rowNumber}: {ex}");
            }

            _logger.LogInformation("Csv parsing successful");
            return items;
        }
    }
}
