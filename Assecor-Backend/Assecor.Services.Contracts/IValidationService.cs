namespace Assecor.Services.Contracts
{
    public interface IValidationService
    {
        bool IsValidEnumValue<T>(int value) where T : Enum;
    }
}
