using Assecor.Backend.Domain.BackendModels;

namespace Assecor.Services.Contracts
{
    public interface IPersonService
    {
        /// <summary>
        /// returns all currently available persons as a list
        /// </summary>
        /// <returns></returns>
        Task<List<Person>> GetAllPersonsAsync();
    }
}
