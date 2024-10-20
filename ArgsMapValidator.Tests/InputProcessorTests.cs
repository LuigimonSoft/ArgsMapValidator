using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArgsMapValidator.Tests.Models;
using ArgsMapValidator.Validation;
using ArgsMapValidator.Tests.Validation;
using ArgsMapValidator.Processing;
using System.Collections.Generic;
using System.Linq;

namespace ArgsMapValidator.Tests
{
    [TestClass]
    public class InputProcessorTests
    {
        private Dictionary<int, string> _errorMessages;
        private InputProcessor<TestModel> _inputProcessor;

        [TestInitialize]
        public void Setup()
        {
            _errorMessages = new Dictionary<int, string>
            {
                { 1001, "Name must not be null." },
                { 1002, "Name must not be empty." },
                 { 1003, "Name must not be empty." },
                 { 1004, "Age must be between 18 and 65." },
                { 1010, "Age must be numeric" },
                { 1005, "Salary must not be null." },
                { 1006, "Salary must be a valid decimal." },
                { 1007, "File path must not be null." },
                { 1008, "File path must not be empty." },
                { 1009, "File path must be a valid path." }
            };

            var validator = new TestModelValidator();
            _inputProcessor = new InputProcessor<TestModel>(validator, _errorMessages);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestCases), DynamicDataSourceType.Method)]
        public void Validate_InputProcessor_DynamicTests(string[] inputs, bool expectedIsValid, string expectedError, int? expectedErrorCode, string testName)
        {
            // Act
            var validationResult = _inputProcessor.Process(inputs);

            // Assert
            Assert.AreEqual(expectedIsValid, validationResult.IsValid, $"Failed Test: {testName}");
            if (!expectedIsValid)
            {
                Assert.AreEqual(expectedError, validationResult.Errors[0].ErrorMessage, $"Failed Test: {testName} - Expected error message not found.");

                // Assert that the error code matches the expected error code
                var actualErrorCode = validationResult.Errors[0].ErrorCode;
                Assert.AreEqual(expectedErrorCode, actualErrorCode, $"Failed Test: {testName} - Expected error code {expectedErrorCode}, but got {actualErrorCode}.");
            }
            else
                Assert.IsTrue(validationResult.IsValid, $"Failed Test: {testName}");
        }

        public static IEnumerable<object[]> GetTestCases()
        {
#if WINDOWS
            yield return new object[]
            {
                new string[] { "John Doe", "30", "5000.75", @"C:\Valid\Path\To\File.txt" }, // Inputs
                true, // Expected IsValid
                null, // Expected Error Message (no error)
                null, // Expected Error Code (no error)
                "All inputs valid" // Test name
            };
#else
            yield return new object[]
            {
                new string[] { "John Doe", "30", "5000.75", @"/Valid/Path/To/File.txt" }, // Inputs
                true, // Expected IsValid
                null, // Expected Error Message (no error)
                null, // Expected Error Code (no error)
                "All inputs valid" // Test name
            };
#endif
            yield return new object[]
            {
                new string[] { null, "30", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Name must not be null.",
                1001,
                "Name is null"
            };

            yield return new object[]
            {
                new string[] { "", "30", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Name must not be empty.",
                1002,
                "Name is empty"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "17", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Age must be between 18 and 65.",
                1004,
                "Age below range"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Age must be numeric",
                1010,
                "Age is empty"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "80", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Age must be between 18 and 65.",
                1004,
                "Age is out of range"
            };
            yield return new object[]
            {
                new string[] { "John Doe", "invalid", "5000.75", @"C:\Valid\Path\To\File.txt" },
                false,
                "Age must be numeric",
                1010,
                "Age is invalid"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "30", "invalid_decimal", @"C:\Valid\Path\To\File.txt" },
                false,
                "Salary must be a valid decimal.",
                1006,
                "Salary is not decimal"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "30", "5000.75", @"Invalid|Path\To\File.txt" },
                false,
                "File path must be a valid path.",
                1009,
                "FilePath contains invalid characters"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "30", "5000.75", null },
                false,
                "File path must not be null.",
                1007,
                "FilePath is null"
            };

            yield return new object[]
            {
                new string[] { "John Doe", "30", "5000.75", "" },
                false,
                "File path must not be empty.",
                1008,
                "FilePath is empty"
            };
        }
    }
}