using System;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Validation.ErrorMessages;

namespace ArgsMapValidator.Validation.Validators
{
    public class NotEmptyValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, object> _property;
        public int? ErrorCode { get; set; }

        public NotEmptyValidationRule(Func<T, object> property)
        {
            _property = property;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);

            if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
            {
                if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var errorMessage))
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = errorMessage });
                }
                else
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = "The value must not be empty or consist solely of whitespace." });
                }
            }
        }

        public void ValidateValue(string value, ValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.AddError(new Error()
                {
                    ErrorCode = ErrorCode ?? 0,
                    ErrorMessage = ErrorMessageStore.GetMessage(ErrorCode ?? 0)
                });
            }
        }
    }
}