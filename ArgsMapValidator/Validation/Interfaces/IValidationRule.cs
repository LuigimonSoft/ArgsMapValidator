namespace ArgsMapValidator.Validation.Interfaces
{
    public interface IValidationRule<T>
    {
        void Validate(T entity, ValidationResult result);
        void ValidateValue(string value, ValidationResult result);
        int? ErrorCode { get; set; }
    }
}

