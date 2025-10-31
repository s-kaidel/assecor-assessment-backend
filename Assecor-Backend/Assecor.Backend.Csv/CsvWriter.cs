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
        /// <param name="records">the objects to append</param>
        /// <returns></returns>
        /// <exception cref="CsvWriterException"></exception>
        public async Task AppendToCsvAsync(string filePath, IEnumerable<T> records)
        {
            try
            {
                var hasRecords = FileHasRecords(filePath);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false, // prohibit writing of duplicate header
                    Delimiter = ", " //add whitespace to delimiter
                };

                await using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                await using var writer = new StreamWriter(stream);
                await using var csv = new CsvHelper.CsvWriter(writer, config);

                if (hasRecords)
                {
                    //skip to next line to avoid writing into an existing row/record
                    await csv.NextRecordAsync();
                }
                await csv.WriteRecordsAsync(records);
            }
            catch (Exception ex)
            {
                throw new CsvWriterException($"An error occurred while trying to write data to the csv file: {ex}");
            }
        }

        private static bool FileHasRecords(string filePath)
        {
            var hasRecords = File.ReadLines(filePath).Skip(1).Any();
            return hasRecords;
        }
    }
}
