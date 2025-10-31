using CsvHelper.Configuration;
using System.Globalization;
using Assecor.Backend.Domain.Exceptions;

namespace Assecor.Backend.CsvAccess
{
    public class CsvWriter
    {
        public async Task WriteToCsvAsync<T>(string filePath, IEnumerable<T> data)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false // prohibit writing of duplicate header
                };

                await using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                await using var writer = new StreamWriter(stream);
                await using var csv = new CsvHelper.CsvWriter(writer, config);

                await csv.WriteRecordsAsync(data);
            }
            catch (Exception ex)
            {
                throw new CsvWriterException($"An error occurred while trying to write data to the csv file: {ex}");
            }
        }
    }
}
