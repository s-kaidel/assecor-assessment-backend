namespace Assecor.Backend.CsvAccess.Interfaces
{
    public interface ICsvWriter<in T>
    {
        /// <summary>
        /// Appends records to a csv file. Returns the number of the first line that was written.
        /// </summary>
        /// <param name="filePath">path to the csv file</param>
        /// <param name="records">the objects to append</param>
        /// <param name="fileHasHeaderRow">to signal if file has an existing header row, defaults to false</param>
        /// <param name="writeHeaderRow">if true, an auto-generated header row matching the types property names will be written. defaults to false</param>
        /// <returns></returns>
        Task<int> AppendToCsvAsync(string filePath, IEnumerable<T> records, bool fileHasHeaderRow = false, bool writeHeaderRow = false);
    }
}
