using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class NotEmptyValidationRuleTests
    {
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _errorMessages = new Dictionary<int, string>
            {
                { 5001, "The value must not be empty or consist solely of whitespace." }
            };
            ErrorMessageStore.Messages = _errorMessages;
        }

        [TestMethod]
        public void Validate_WhenValueIsNotEmpty_ShouldPass()
        {
            // Arrange
            var rule = new NotEmptyValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 5001
            };
            var testObject = new TestObject { Value = "Valid String" };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsEmpty_ShouldFail()
        {
            // Arrange
            var rule = new NotEmptyValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 5001
            };
            var testObject = new TestObject { Value = "" };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The value must not be empty or consist solely of whitespace.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(5001, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenValueIsWhitespace_ShouldFail()
        {
            // Arrange
            var rule = new NotEmptyValidationRule<TestObject>(x => x.Value)
            {
                ErrorCode = 5001
            };
            var testObject = new TestObject { Value = "   " };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The value must not be empty or consist solely of whitespace.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(5001, result.Errors[0].ErrorCode);
        }

        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}