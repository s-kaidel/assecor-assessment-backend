using Assecor.Services.Contracts;

namespace Assecor.Backend.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidEnumValue<T>(int value) where T : Enum
        {
            var isValid = Enum.IsDefined(typeof(T), value);
            return isValid;
        }
    }
}
