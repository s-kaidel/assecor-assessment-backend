namespace Assecor.Backend.CsvAccess.Interfaces
{
    public interface ICsvWriter<in T>
    {
        Task WriteToCsvAsync(string filePath, IEnumerable<T> data);
    }
}
