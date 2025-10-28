using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;

namespace Assecor.Backend.Dal.Helper
{
    public class CsvReaderHelper
    {
        /// <summary>
        /// Converts a single row to a CsvPerson, missing or faulty values will be null
        /// </summary>
        /// <param name="fields">values of the currently read row</param>
        /// <param name="rowNumber">the current row</param>
        /// <returns></returns>
        public CsvPerson? GetPersonFromCsvRow(IEnumerable<string> fields, int rowNumber)
        {
            string? name = null;
            string? lastName = null;
            string? city = null;
            int? zipCode = null;
            Color? color = null;

            var fieldsList = fields.ToList();

            if (fieldsList.Count == 0)
            {
                return null;
            }

            foreach (var field in fieldsList)
            {
                var value = field.Trim();

                if (string.IsNullOrEmpty(city)
                    && zipCode == null
                    && char.IsDigit(value.FirstOrDefault()))
                {
                    var location = ParseLocation(value);
                    city = location.city;
                    zipCode = location.zipCode;
                    continue;
                }

                if (color is null 
                    && int.TryParse(value, out var colorValue) 
                    && Enum.IsDefined(typeof(Color), colorValue))
                {
                    color = (Color)colorValue;
                    continue;
                }

                if (name == null)
                {
                    name = value;
                    continue;
                }

                lastName ??= value;
            }

            var person = new CsvPerson()
            {
                Id = rowNumber,
                Name = name,
                LastName = lastName,
                City = city,
                ZipCode = zipCode,
                Color = color,
            };
            return person;
        }
        private (int? zipCode, string? city) ParseLocation(string location)
        {
            var commaIndex = location.IndexOf(' ');

            var zipCodeString = location.Substring(0, commaIndex).Trim();
            var city = location.Substring(commaIndex + 1).Trim();

            if (int.TryParse(zipCodeString, out var zipCode))
            {
                return (zipCode, city);
            }

            return (null, city);
        }
    }
}
