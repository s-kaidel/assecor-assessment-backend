using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;

namespace Assecor.Backend.Dal.Contracts
{
    public interface ICsvPersonProvider
    {
        /// <summary>
        /// Reads all person records from a csv file and returns them as a list
        /// </summary>
        /// <returns></returns>
        Task<List<CsvPerson>> ReadPersonsFromCsvAsync();

        /// <summary>
        /// Returns all persons whose favorite color is provided color
        /// </summary>
        /// <param name="color">the color to match</param>
        /// <returns></returns>
        Task<List<CsvPerson>> GetPersonsByColorAsync(Color color);

        /// <summary>
        /// Returns the person matching provided id. If none found, null is returned
        /// </summary>
        /// <param name="id">the id to match</param>
        /// <returns></returns>
        Task<CsvPerson?> GetPersonByIdAsync(int id);
    }
}
