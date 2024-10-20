using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class RangeValidationRuleTests
    {
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _errorMessages = new Dictionary<int, string>
            {
                { 3001, "Value must be within the specified range." }
            };
            ErrorMessageStore.Messages = _errorMessages;
        }

        [TestMethod]
        public void Validate_WhenValueIsWithinRange_ShouldPass()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, int>(x => x.NumericValue, 1, 10)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { NumericValue = 5 };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsBelowMinimum_ShouldFail()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, int>(x => x.NumericValue, 1, 10)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { NumericValue = 0 };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be within the specified range.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(3001, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenValueIsAboveMaximum_ShouldFail()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, int>(x => x.NumericValue, 1, 10)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { NumericValue = 15 };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be within the specified range.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(3001, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenValueIsEqualToMinimum_ShouldPass()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, int>(x => x.NumericValue, 1, 10)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { NumericValue = 1 };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsEqualToMaximum_ShouldPass()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, int>(x => x.NumericValue, 1, 10)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { NumericValue = 10 };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsWithinRange_ForDecimal_ShouldPass()
        {
            // Arrange
            var rule = new RangeValidationRule<TestObject, decimal>(x => x.DecimalValue, 1.0m, 10.0m)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { DecimalValue = 5.5m };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenValueIsOutOfRange_ForDateTime_ShouldFail()
        {
            // Arrange
            var minDate = new DateTime(2020, 1, 1);
            var maxDate = new DateTime(2022, 1, 1);
            var rule = new RangeValidationRule<TestObject, DateTime>(x => x.DateValue, minDate, maxDate)
            {
                ErrorCode = 3001
            };
            var testObject = new TestObject { DateValue = new DateTime(2023, 1, 1) };
            var result = new ValidationResult();

            // Act
            rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be within the specified range.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(3001, result.Errors[0].ErrorCode);
        }

        // Helper class to use in tests
        private class TestObject
        {
            public int NumericValue { get; set; }
            public decimal DecimalValue { get; set; }
            public DateTime DateValue { get; set; }
        }
    }
}