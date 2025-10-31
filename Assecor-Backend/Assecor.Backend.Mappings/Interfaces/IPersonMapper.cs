using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.DalModels;

namespace Assecor.Backend.Mappings.Interfaces
{
    public interface IPersonMapper
    {
        List<Person> MapFromCsvPersons(IEnumerable<CsvPerson> csvPersons);
        Person MapFromCsvPerson(CsvPerson csvPerson);
    }
}
