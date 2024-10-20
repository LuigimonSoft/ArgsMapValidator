using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class LengthValidationRuleTests
    {
        private LengthValidationRule<TestObject> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new LengthValidationRule<TestObject>(x => x.Value, 1, 5);
            _errorMessages = new Dictionary<int, string> { { 1005, "Length must be between 1 and 5 characters." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1005;
        }

        [TestMethod]
        public void Validate_WhenLengthIsWithinRange_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { Value = "abc" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenLengthIsBelowMinimum_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { Value = "" }; // Length is 0
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Length must be between 1 and 5 characters.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(1005, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenLengthIsAboveMaximum_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { Value = "abcdef" }; // Length is 6
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Length must be between 1 and 5 characters.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(1005, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenValueIsNull_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject { Value = null };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid); // Null value should pass for Length rule
        }

        private class TestObject
        {
            public string Value { get; set; }
        }
    }
}