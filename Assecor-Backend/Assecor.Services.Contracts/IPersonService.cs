using Assecor.Backend.Domain.BackendModels;

namespace Assecor.Services.Contracts
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllPersonsAsync();
    }
}
