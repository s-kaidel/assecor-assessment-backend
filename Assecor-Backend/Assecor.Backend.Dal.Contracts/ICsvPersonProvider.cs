using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Maybe;

namespace Assecor.Backend.Dal.Contracts
{
    public interface ICsvPersonProvider
    {
        /// <summary>
        /// Returns all currently available persons from the csv data file.
        /// Persons must have a name and last name, otherwise they are skipped.
        /// </summary>
        /// <returns></returns>
        Task<List<CsvPerson>> GetAllPersonsAsync();

        /// <summary>
        /// Returns all persons whose favorite color matches provided color.
        /// Persons must have a name and last name, otherwise they are skipped.
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        Task<List<CsvPerson>> GetPersonsByColorAsync(Color color);

        /// <summary>
        /// Returns the person matching provided id.
        /// Persons must have a name and last name, otherwise they are skipped.
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        Task<Maybe<CsvPerson>> GetPersonByIdAsync(int id);
    }
}
