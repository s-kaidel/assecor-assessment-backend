using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Mapping;
using Assecor.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Services
{
    public class PersonService(ILogger<PersonService> logger, ICsvProvider csvProvider) : IPersonService
    {
        public List<Person> GetPersons()
        {
            var dalPersons = csvProvider.ReadPersonsFromCsv();
            var persons = PersonMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }
    }
}
