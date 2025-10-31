using Assecor.Backend.CsvAccess.Interfaces;
using Assecor.Backend.Domain.Exceptions;
using CsvHelper.Configuration;
using System.Globalization;

namespace Assecor.Backend.CsvAccess
{
    public class CsvWriter<T> : ICsvWriter<T>
    {
        /// <summary>
        /// Appends records to a csv file. Returns the number of the first line that was written.
        /// </summary>
        /// <param name="filePath">path to the csv file</param>
        /// <param name="records">the objects to append</param>
        /// <param name="fileHasHeaderRow">to signal if file has an existing header row, defaults to false</param>
        /// <param name="writeHeaderRow">if true, an auto-generated header row matching the types property names will be written. defaults to false</param>
        /// <returns></returns>
        /// <exception cref="CsvWriterException"></exception>
        public async Task<int> AppendToCsvAsync(string filePath, IEnumerable<T> records, bool fileHasHeaderRow = false, bool writeHeaderRow = false)
        {
            try
            {
                var lines = GetFileLines(filePath);
                var hasRecords = FileHasRecords(lines);
                var nextLineNumber = GetNextLineValue(lines);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = writeHeaderRow, //false prohibits writing of duplicate header row
                    Delimiter = ", " //add whitespace to delimiter
                };

                await using var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                await using var writer = new StreamWriter(stream);
                await using var csv = new CsvHelper.CsvWriter(writer, config);

                if (hasRecords || fileHasHeaderRow)
                {
                    //skip to next line to avoid writing into an existing row/record
                    await csv.NextRecordAsync();
                }
                await csv.WriteRecordsAsync(records);
                return nextLineNumber;
            }
            catch (Exception ex)
            {
                throw new CsvWriterException($"An error occurred while trying to write data to the csv file: {ex}");
            }
        }

        private static List<string> GetFileLines(string filePath)
        {
            return File.ReadLines(filePath).ToList();
        }

        private static int GetNextLineValue(List<string> fileLines)
        {
            var id = fileLines.Count + 1;
            return id;
        }

        private static bool FileHasRecords(List<string> fileLines)
        {
            var hasRecords = fileLines.Count > 0;
            return hasRecords;
        }
    }
}
