using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class IsNumericValidationRuleTests
    {
        private IsNumericValidationRule<TestObject> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new IsNumericValidationRule<TestObject>(x => x.Value);
            _errorMessages = new Dictionary<int, string> { { 1003, "Value must be numeric." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1003;
        }

        private class TestObject
        {
            public int Value { get; set; }
        }
    }
}

