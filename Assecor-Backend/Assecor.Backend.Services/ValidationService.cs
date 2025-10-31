using Assecor.Backend.Domain.Dto;
using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidEnumValue<T>(int value) where T : Enum
        {
            var isValid = Enum.IsDefined(typeof(T), value);
            return isValid;
        }

        public bool IsValidCsvPerson(CsvPersonDto personDto)
        {
            var isValidEnum = true;
            if (personDto.Color != null)
            {
                isValidEnum = IsValidEnumValue<Color>(personDto.Color.Value);
            }

            var hasName = string.IsNullOrEmpty(personDto.Name);
            var hasLastName = string.IsNullOrEmpty(personDto.LastName);

            var validPerson = hasName && hasLastName && isValidEnum;
            return validPerson;
        }
    }
}
