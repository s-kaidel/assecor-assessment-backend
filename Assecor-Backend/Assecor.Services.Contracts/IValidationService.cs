using Assecor.Backend.Domain.Requests;

namespace Assecor.Backend.Services.Contracts
{
    public interface IValidationService
    {
        /// <summary>
        /// Checks if provided value is in given type of enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>true, if value is in enum</returns>
        bool IsValidEnumValue<T>(int value) where T : Enum;

        /// <summary>
        /// Checks if the request data is valid. Name and lastName are required and the color, if existing, needs to be valid.
        /// If a city is provided, it can't be a whitespace
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns></returns>
        bool IsValidCsvPerson(CreateCsvPersonRequest personRequest);
    }
}
