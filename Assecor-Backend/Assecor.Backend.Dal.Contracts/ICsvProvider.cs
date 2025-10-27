using Assecor.Backend.Domain.DalModels;

namespace Assecor.Backend.Dal.Contracts
{
    public interface ICsvProvider
    {
        Task<List<CsvPerson>> ReadPersonsFromCsvAsync();
    }
}
