using Assecor.Backend.Domain;
using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Enums;

namespace Assecor.Services.Contracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Returns all currently available persons as a list
        /// </summary>
        /// <returns></returns>
        Task<List<Person>> GetAllPersonsAsync();

        /// <summary>
        /// Returns all persons whose favorite color is provided color
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        Task<List<Person>> GetPersonsByColorAsync(Color color);

        /// <summary>
        /// Returns the person matching provided id. If none found, null is returned
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        Task<Maybe<Person>> GetPersonByIdAsync(int id);
    }
}
