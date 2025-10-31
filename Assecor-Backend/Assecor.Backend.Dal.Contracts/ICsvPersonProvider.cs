using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;

namespace Assecor.Backend.Dal.Contracts
{
    public interface ICsvPersonProvider
    {
        /// <summary>
        /// Returns all currently available persons from the csv data file.
        /// </summary>
        /// <returns></returns>
        Task<List<CsvPerson>> GetAllPersonsAsync();

        /// <summary>
        /// Returns all persons whose favorite color matches provided color.
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        Task<List<CsvPerson>> GetPersonsByColorAsync(Color color);

        /// <summary>
        /// Returns the person matching provided id.
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        Task<Maybe<CsvPerson>> GetPersonByIdAsync(int id);

        /// <summary>
        /// Create a person entry in the csv data file. Returns the id of created person.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> CreateCsvPersonAsync(CsvPersonDto dto);
    }
}
