using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Validation.Validators;
using ArgsMapValidator.Validation.ErrorMessages;
using System.Collections.Generic;

namespace ArgsMapValidator.Tests.Validation.Validators
{
    [TestClass]
    public class IsPathValidationRuleTests
    {
        private IsPathValidationRule<TestObject> _rule;
        private Dictionary<int, string> _errorMessages;

        [TestInitialize]
        public void Setup()
        {
            _rule = new IsPathValidationRule<TestObject>(x => x.PathValue);
            _errorMessages = new Dictionary<int, string> { { 1007, "Value must be a valid path." } };
            ErrorMessageStore.Messages = _errorMessages;
            _rule.ErrorCode = 1007;
        }

        [TestMethod]
        public void Validate_WhenPathIsValid_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject();
            #if WINDOWS
            testObject.PathValue = @"c:\Valid\Path\To\File.txt";
            #else
            testObject.PathValue = "/Valid/Path/To/File.txt";
            #endif
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenPathIsValidRelativePath_ShouldPass()
        {
            // Arrange
            var testObject = new TestObject(); 
            #if WINDOWS
            testObject.PathValue = @"..\Valid\Relative\Path\File.txt";
            #else
            testObject.PathValue = @"../valid/relative/path/file.txt";
            #endif
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_WhenPathIsInvalid_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject(); 
            #if WINDOWS
            testObject.PathValue = @"C:\Invalid|Path\To\File.txt";
            #else
            testObject.PathValue = @"C:/Invalid|Path/To/File.txt";
            #endif
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be a valid path.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(1007, result.Errors[0].ErrorCode);
        }

        [TestMethod]
        public void Validate_WhenValueIsNotAPath_ShouldFail()
        {
            // Arrange
            var testObject = new TestObject { PathValue = "hello world" };
            var result = new ValidationResult();

            // Act
            _rule.Validate(testObject, result);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Value must be a valid path.", result.Errors[0].ErrorMessage);
            Assert.AreEqual(1007, result.Errors[0].ErrorCode);
        }

        private class TestObject
        {
            public string PathValue { get; set; }
        }
    }
}