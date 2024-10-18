using System;
using ArgsMapValidator.Validation.Interfaces;

namespace ArgsMapValidator.Validation.Validators
{
    public class RuleBuilder<T, TProperty>
    {
        private readonly AbstractValidator<T> _validator;
        private readonly Func<T, TProperty> _property;
        private readonly int _position;

        public RuleBuilder(AbstractValidator<T> validator, Func<T, TProperty> property, int position)
        {
            _validator = validator;
            _property = property;
            _position = position;
        }

        public RuleBuilder<T, TProperty> NotNull()
        {
            var rule = new NotNullValidationRule<T, TProperty>(_property);
            _validator.AddRule(rule, _position);
            return this;
        }

        public RuleBuilder<T, TProperty> NotEmpty()
        {
            var rule = new NotEmptyValidationRule<T>(_property as Func<T, object>);
            _validator.AddRule(rule, _position);
            return this;
        }

        public RuleBuilder<T, TProperty> IsPath()
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new IsPathValidationRule<T>(stringProperty);
                _validator.AddRule(rule, _position);
            }
            else
            {
                throw new InvalidOperationException("IsPath can only be used with string properties.");
            }

            return this;
        }


        public RuleBuilder<T, TProperty> InRange(TProperty min, TProperty max)
        {
            var rule = new RangeValidationRule<T, TProperty>(_property, min, max);
            _validator.AddRule(rule, _position);
            return this;
        }

        public RuleBuilder<T, TProperty> IsDecimal()
        {
            var rule = new IsDecimalValidationRule<T>(_property as Func<T, decimal>);
            _validator.AddRule(rule, _position);
            return this;
        }

        public RuleBuilder<T, TProperty> Length(int minLength, int maxLength)
        {
            if (_property is Func<T, string> stringProperty)
            {
                var rule = new LengthValidationRule<T>(stringProperty, minLength, maxLength);
                _validator.AddRule(rule, _position);
            }
            else
            {
                throw new InvalidOperationException("Length validation can only be applied to string properties.");
            }
            return this;
        }

        public RuleBuilder<T, TProperty> IsNumeric()
        {
            var rule = new IsNumericValidationRule<T>(_property as Func<T, int>);//object>);
            _validator.AddRule(rule, _position);
            return this;
        }

        public RuleBuilder<T, TProperty> WithErrorCode(int errorCode)
        {
            var lastRule = _validator.GetRulesForPosition(_position).LastOrDefault();
            if (lastRule != null)
            {
                lastRule.ErrorCode = errorCode;
            }
            return this;
        }
    }
}


