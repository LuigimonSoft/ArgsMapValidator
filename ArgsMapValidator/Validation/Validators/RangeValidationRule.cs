using System;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Validation.ErrorMessages;

namespace ArgsMapValidator.Validation.Validators
{
    public class RangeValidationRule<T, TProperty> : IValidationRule<T>
    {
        private readonly Func<T, TProperty> _property;
        private readonly TProperty _min;
        private readonly TProperty _max;
        public int? ErrorCode { get; set; }

        public RangeValidationRule(Func<T, TProperty> property, TProperty min, TProperty max)
        {
            _property = property;
            _min = min;
            _max = max;
        }

        public void Validate(T entity, ValidationResult result)
        {
            var value = _property(entity);

            if (value is IComparable comparableValue && _min is IComparable minComparable && _max is IComparable maxComparable)
            {
                if (comparableValue.CompareTo(minComparable) < 0 || comparableValue.CompareTo(maxComparable) > 0)
                {
                    if (ErrorCode.HasValue && ErrorMessageStore.Messages.TryGetValue(ErrorCode.Value, out var errorMessage))
                    {
                        result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = string.Format(errorMessage, _min, _max) });
                    }
                    else
                    {
                        result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = $"The value must be between {_min} and {_max}." });
                    }
                }
            }
            else
            {
                result.AddError(new Error() { ErrorCode = ErrorCode.Value, ErrorMessage = "The value must be comparable." });
            }
        }

        public void ValidateValue(string value, ValidationResult result)
        {
            try
            {
                if (typeof(TProperty) == typeof(int))
                {
                    var intValue = int.Parse(value);
                    if (intValue.CompareTo((int)(object)_min) < 0 || intValue.CompareTo((int)(object)_max) > 0)
                    {
                        result.AddError(new Error()
                        {
                            ErrorCode = ErrorCode ?? 0,
                            ErrorMessage = ErrorMessageStore.GetMessage(ErrorCode ?? 0)
                        });
                    }
                }
                else if (typeof(TProperty) == typeof(decimal))
                {
                    var decimalValue = decimal.Parse(value);
                    if (decimalValue.CompareTo((decimal)(object)_min) < 0 || decimalValue.CompareTo((decimal)(object)_max) > 0)
                    {
                        result.AddError(new Error()
                        {
                            ErrorCode = ErrorCode ?? 0,
                            ErrorMessage = ErrorMessageStore.GetMessage(ErrorCode ?? 0)
                        });
                    }
                }
                else if (typeof(TProperty) == typeof(DateTime))
                {
                    var dateValue = DateTime.Parse(value);
                    if (dateValue.CompareTo((DateTime)(object)_min) < 0 || dateValue.CompareTo((DateTime)(object)_max) > 0)
                    {
                        result.AddError(new Error()
                        {
                            ErrorCode = ErrorCode ?? 0,
                            ErrorMessage = ErrorMessageStore.GetMessage(ErrorCode ?? 0)
                        });
                    }
                }
            }
            catch (FormatException)
            {
                result.AddError(new Error()
                {
                    ErrorCode = ErrorCode ?? 0,
                    ErrorMessage = $"Invalid format for range validation for value: {value}"
                });
            }
        }
    }
}
