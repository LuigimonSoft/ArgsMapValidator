using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArgsMapValidator.Mapping;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.ErrorMessages;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Attributes;
using ArgsMapValidator.Validation.Validators;

namespace ArgsMapValidator.Processing
{
    public class InputProcessor<T> where T : new()
    {
        private readonly IValidator<T> _validator;

        public InputProcessor(IValidator<T> validator, Dictionary<int, string> errorMessages)
        {
            _validator = validator;
            ErrorMessageStore.Messages = errorMessages;
        }

        public ValidationResult Process(string[] parameters)
        {
            var preValidationResult = PreValidateInputs(parameters);
            if (!preValidationResult.IsValid)
            {
                return preValidationResult;
            }

            try
            {
                T model = ObjectMapper.MapFromParameters<T>(parameters);

                return _validator.Validate(model);
            }
            catch (Exception ex)
            {
                var result = new ValidationResult();
                result.AddError(new Error() { ErrorCode = 0, ErrorMessage = $"Error when mapping: {ex.Message}" });
                return result;
            }
        }

        private ValidationResult PreValidateInputs(string[] parameters)
        {
            var validationResult = new ValidationResult();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var parameterAttr = prop.GetCustomAttribute<ParameterAttribute>();
                if (parameterAttr == null) continue;

                var index = parameterAttr.Index;

                if (index >= parameters.Length)
                {
                    validationResult.AddError(new Error()
                    {
                        ErrorCode = 0,
                        ErrorMessage = $"The input parameter at index {index} is missing for the property '{prop.Name}'."
                    });
                    continue;
                }

                var parameterValue = parameters[index];

                var rules = (_validator as AbstractValidator<T>).GetRulesForPosition(index);

                foreach (var rule in rules)
                {
                    rule.ValidateValue(parameterValue, validationResult);
                }
            }

            return validationResult;
        }
    }
}
