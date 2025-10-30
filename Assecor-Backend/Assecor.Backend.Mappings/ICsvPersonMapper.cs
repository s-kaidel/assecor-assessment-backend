using Assecor.Backend.Domain.DalModels;

namespace Assecor.Backend.Mappings
{

    public interface ICsvPersonMapper
    {
        /// <summary>
        /// Converts a single row to a CsvPerson, missing or faulty values will be null. At least a name and last name are expected, otherwise will be null;
        /// </summary>
        /// <param name="fields">values of the currently read row</param>
        /// <param name="rowNumber">the current row</param>
        /// <returns></returns>
        CsvPerson? MapFromCsvRow(IEnumerable<string> fields, int rowNumber);
    }
}
