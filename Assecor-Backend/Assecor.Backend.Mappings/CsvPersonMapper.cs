using Assecor.Backend.Domain.DalModels;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Mappings
{
    public class CsvPersonMapper(ILogger<CsvPersonMapper> logger, IValidationService validationService) : ICsvPersonMapper
    {
        private readonly ILogger<CsvPersonMapper> _logger = logger;
        private readonly IValidationService _validationService = validationService;

        /// <summary>
        /// Converts a single row to a CsvPerson, missing or faulty values will be null. At least a name and last name are expected, otherwise will be null;
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
                    var location = ParseLocation(value, rowNumber);
                    if (location.city != null)
                    { 
                        city = location.city;
                        zipCode = location.zipCode;
                        continue;
                    }
                }

                if (color is null)
                {
                    var validation = IsValidEnumValue(field, rowNumber);
                    if (validation.isValid)
                    {
                        color = (Color)validation.colorValue!;
                        continue;
                    }
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
            
            return IsValidPerson(person, rowNumber) ? person : null;
        }

        private (int? zipCode, string? city) ParseLocation(string location, int rowNumber)
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
                _logger.LogInformation("string '{zipCodeStr}' in row {rowNumber} was not parsable to int, zipCode will be null", zipCode, rowNumber);
                return (null, city);
            }

            _logger.LogInformation("string '{location}' in row {rowNumber} is not parsable to zipCode and city, they will be null", location, rowNumber);
            return (null, null);
        }

        private (bool isValid, int? colorValue) IsValidEnumValue(string color, int rowNumber)
        {
            var isValidEnum = false;
            var isParsable = int.TryParse(color, out var colorValue);

            if (isParsable)
            {
                isValidEnum = _validationService.IsValidEnumValue<Color>(colorValue);
            }

            if (isValidEnum)
            {
                return (true, colorValue);
            }
            _logger.LogInformation("Value '{color}' in row {rowNumber} is no value of enum '{enum}'", color, rowNumber, nameof(Color));
            return (false, null);
        }

        private bool IsValidPerson(CsvPerson person, int rowNumber)
        {
            var hasName = !string.IsNullOrEmpty(person.Name);
            var hasLastName = !string.IsNullOrEmpty(person.LastName);

            var valid = hasName && hasLastName;
            if (!valid)
            {
                _logger.LogInformation("Person in row {row} has no name and last name, person is skipped", rowNumber);
            }

            return valid;
        }
    }
}
