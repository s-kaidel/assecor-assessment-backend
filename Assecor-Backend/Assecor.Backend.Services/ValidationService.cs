using Assecor.Backend.Domain.Enums;
using Assecor.Backend.Domain.Requests;
using Assecor.Backend.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Checks if provided value is in given type of enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>true, if value is in enum</returns>
        public bool IsValidEnumValue<T>(int value) where T : Enum
        {
            var isValid = Enum.IsDefined(typeof(T), value);
            return isValid;
        }


        //TODO add logging, return string containing faulty values
        /// <summary>
        /// Checks if the request data is valid. Name and lastName are required and the color, if existing, needs to be valid.
        /// If a city is provided, it can't be a whitespace
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns></returns>
        public bool IsValidCsvPerson(CreateCsvPersonRequest personRequest)
        {
            var isValidEnum = true;
            if (personRequest.Color != null)
            {
                isValidEnum = IsValidEnumValue<Color>(personRequest.Color.Value);
            }

            var hasName = !string.IsNullOrEmpty(personRequest.Name);
            var hasLastName = !string.IsNullOrEmpty(personRequest.LastName);
            var validCity = personRequest.City == null || !string.IsNullOrWhiteSpace(personRequest.City);

            var validPerson = hasName && hasLastName && isValidEnum && validCity;
            return validPerson;
        }
    }
}
