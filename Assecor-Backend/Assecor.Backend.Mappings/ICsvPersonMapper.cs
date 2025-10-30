using Assecor.Backend.Domain.DalModels;

namespace Assecor.Backend.Mappings
{
    public interface ICsvPersonMapper
    {
        CsvPerson? MapFromCsvRow(IEnumerable<string> fields, int rowNumber);
    }
}
