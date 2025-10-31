using Assecor.Backend.Domain.BackendModels;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;

namespace Assecor.Backend.Services.Contracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Returns all currently available persons from the data storage
        /// </summary>
        /// <returns></returns>
        Task<List<Person>> GetAllPersonsAsync();

        /// <summary>
        /// Returns all persons whose favorite color matches provided color
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        Task<List<Person>> GetPersonsByColorAsync(Color color);

        /// <summary>
        /// Returns the person matching provided id. If none found, an empty maybe is returned
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        Task<Maybe<Person>> GetPersonByIdAsync(int id);

        /// <summary>
        /// Create a new person entry in the data storage, returns id of created person
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> CreateNewCsvPersonAsync(CsvPersonDto dto);
    }
}
