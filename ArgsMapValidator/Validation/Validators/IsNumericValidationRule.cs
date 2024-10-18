using System;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Validation.ErrorMessages;

namespace ArgsMapValidator.Validation.Validators
{
    public class IsNumericValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, int> _property;
        public int? ErrorCode { get; set; }

        public IsNumericValidationRule(Func<T, int> property)
        {
            _property = property;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity).ToString();
            if (value != null && !int.TryParse(value, out _))
            {
                if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var erroMessage))
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = erroMessage });
                }
                else
                {
                    result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = "The value must be numeric." });
                }

            }
        }

        public void ValidateValue(string value, ValidationResult result)
        {
            if (!int.TryParse(value, out _))
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