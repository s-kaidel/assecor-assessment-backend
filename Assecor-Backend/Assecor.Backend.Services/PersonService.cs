using Assecor.Backend.Dal.Contracts;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Mapping;
using Assecor.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Services
{
    public class PersonService(ILogger<PersonService> logger, ICsvProvider csvProvider) : IPersonService
    {
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            var dalPersons = await csvProvider.ReadPersonsFromCsvAsync();
            var persons = PersonMapper.MapFromCsvPersons(dalPersons);
            return persons;
        }
    }
}
