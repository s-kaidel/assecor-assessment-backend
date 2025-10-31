namespace Assecor.Backend.CsvAccess.Interfaces
{
    public interface ICsvWriter<in T>
    {
        Task AppendToCsvAsync(string filePath, IEnumerable<T> data);
    }
}
