using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Domain.Exceptions;
using CsvHelper.Configuration;
using System.Globalization;

namespace Assecor.Backend.CsvAccess
{
    public class CsvWriter<T> : ICsvWriter<T>
    {
        /// <summary>
        /// Appends records to a csv file, headers are ignored
        /// </summary>
        /// <param name="filePath">path to the csv file</param>
        /// <param name="data">the objects to append</param>
        /// <returns></returns>
        /// <exception cref="CsvWriterException"></exception>
        public async Task AppendToCsvAsync(string filePath, IEnumerable<T> data)
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
