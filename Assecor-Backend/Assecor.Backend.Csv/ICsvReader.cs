namespace Assecor.Backend.CsvAccess
{
    public interface ICsvReader<T>
    {
        Task<List<T>> ReadFromCsvAsync(Func<IEnumerable<string>, int, T?> mappingFunc, string filePath);
    }
}
