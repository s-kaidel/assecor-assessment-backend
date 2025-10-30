using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Mappings
{
    public class CsvPersonMapper(ILogger<CsvPersonMapper> logger) : ICsvPersonMapper
    {
        /// <summary>
        /// Converts a single row to a CsvPerson, missing or faulty values will be null
        /// </summary>
        /// <param name="fields">values of the currently read row</param>
        /// <param name="rowNumber">the current row</param>
        /// <returns></returns>
        public CsvPerson? MapFromCsvRow(IEnumerable<string> fields, int rowNumber)
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
                    if (location.city != null)
                    { 
                        city = location.city;
                        zipCode = location.zipCode;
                        continue;
                    }
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
        private static (int? zipCode, string? city) ParseLocation(string location)
        {
            var whiteSpaceIndex = location.IndexOf(' ');
            if (whiteSpaceIndex > 0)
            {
                var zipCodeString = location.Substring(0, whiteSpaceIndex).Trim();
                var city = location.Substring(whiteSpaceIndex + 1).Trim();

                if (int.TryParse(zipCodeString, out var zipCode))
                {
                    return (zipCode, city);
                }

                return (null, city);
            }

            return (null, null);
        }
    }
}
