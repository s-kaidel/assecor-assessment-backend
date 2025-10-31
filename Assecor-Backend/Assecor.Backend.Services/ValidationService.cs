using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Requests;
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

        public bool IsValidCsvPerson(CreateCsvPersonRequest personRequest)
        {
            var isValidEnum = true;
            if (personRequest.Color != null)
            {
                isValidEnum = IsValidEnumValue<Color>(personRequest.Color.Value);
            }

            var hasName = string.IsNullOrEmpty(personRequest.Name);
            var hasLastName = string.IsNullOrEmpty(personRequest.LastName);

            var validPerson = hasName && hasLastName && isValidEnum;
            return validPerson;
        }
    }
}
