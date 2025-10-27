using Assecor.Backend.Domain.BackendModels;

namespace Assecor.Services.Contracts
{
    public interface IPersonService
    {
        List<Person> GetPersons();
    }
}
