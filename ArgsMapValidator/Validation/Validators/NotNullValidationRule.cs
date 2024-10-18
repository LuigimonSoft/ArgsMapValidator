using System;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Validation.ErrorMessages;

namespace ArgsMapValidator.Validation.Validators
{
    public class NotNullValidationRule<T, TProperty> : IValidationRule<T>
    {
        private readonly Func<T, TProperty> _property;
        public int? ErrorCode { get; set; }

        public NotNullValidationRule(Func<T, TProperty> property)
        {
            _property = property;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);
            if (value == null)
            {
                if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var errorMessage))
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = errorMessage });
                }
                else
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = "The value cannot be null." });
                }
            }
        }

        public void ValidateValue(string value, ValidationResult result)
        {
            if (value == null)
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

