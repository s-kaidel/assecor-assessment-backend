namespace Assecor.Backend.Services.Contracts
{
    public interface IValidationService
    {
        /// <summary>
        /// Checks if provided value is in given type of enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsValidEnumValue<T>(int value) where T : Enum;
    }
}
