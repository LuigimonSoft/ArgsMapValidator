using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ArgsMapValidator.Validation.Interfaces;
using ArgsMapValidator.Attributes;

namespace ArgsMapValidator.Validation.Validators
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        private readonly Dictionary<int, List<IValidationRule<T>>> _positionRules = new Dictionary<int, List<IValidationRule<T>>>();

        public ValidationResult Validate(T instance)
        {
            var result = new ValidationResult();

            foreach (var rules in _positionRules.Values)
            {
                foreach (var rule in rules)
                {
                    rule.Validate(instance, result);
                }
            }

            return result;
        }

        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("The expression of ownership is not valid");
            }

            var propertyInfo = typeof(T).GetProperty(memberExpression.Member.Name);
            var parameterAttribute = propertyInfo.GetCustomAttribute<ParameterAttribute>();

            if (parameterAttribute == null)
            {
                throw new InvalidOperationException($"The property '{propertyInfo.Name}' does not have a parameterAttribute attribute.");
            }

            return new RuleBuilder<T, TProperty>(this, propertyExpression.Compile(), parameterAttribute.Index);
        }

        public void AddRule(IValidationRule<T> rule, int position)
        {
            if (!_positionRules.ContainsKey(position))
            {
                _positionRules[position] = new List<IValidationRule<T>>();
            }
            _positionRules[position].Add(rule);
        }

        public IEnumerable<IValidationRule<T>> GetRulesForPosition(int position)
        {
            return _positionRules.ContainsKey(position) ? _positionRules[position] : Enumerable.Empty<IValidationRule<T>>();
        }
    }
}
