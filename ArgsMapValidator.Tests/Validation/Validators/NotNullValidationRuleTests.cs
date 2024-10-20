using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class NotNullValidationRuleTests
    {
        private NotNullValidationRule<TestObject, string> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new NotNullValidationRule<TestObject, string>(x => x.Value);
            _errorMessages = new Dictionary<int, string> { { 1001, "Value cannot be null." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1001;
        }

        [TestMethod]
        public void Validate_WhenValueIsNotNull_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { Value = "Valid Value" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsNull_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { Value = null };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value cannot be null.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(1001, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenErrorCodeDoesNotExist_ShouldUseDefaultMessage()
        {
            // Arrange
            var testObject = new TestObject { Value = null };
            var result = new ValidationResult();
            _rule.ErrorCode = 9999; 

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The value cannot be null.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(9999, result.Errors[0].ErrorCode);
        }

        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}