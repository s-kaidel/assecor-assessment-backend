namespace Assecor.Backend.CsvAccess
{
    public interface ICsvReader<T>
    {
        /// <summary>
        /// Tries to parse a csv file with provided mapping method
        /// </summary>
        /// <param name="mappingFunc">the mapping method to use while parsing</param>
        /// <param name="filePath">the full filePath</param>
        /// <returns></returns>
        Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc, string filePath);
    }
}
