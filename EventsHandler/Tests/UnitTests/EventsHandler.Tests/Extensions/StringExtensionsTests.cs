﻿// © 2024, Worth Systems.

using System.Text;
using EventsHandler.Extensions;

namespace EventsHandler.UnitTests.Extensions
{
    [TestFixture]
    public sealed class StringExtensionsTests
    {
        #region Validation
        [TestCase("", true)]
        [TestCase(" ", false)]
        [TestCase("1", false)]
        [TestCase("abc", false)]
        public void IsEmpty_ReturnsExpectedResult(string testString, bool expectedResult)
        {
            // Act
            bool actualResult = testString.IsEmpty();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("1", true)]
        [TestCase("abc", true)]
        public void IsNotEmpty_ReturnsExpectedResult(string testString, bool expectedResult)
        {
            // Act
            bool actualResult = testString.IsNotEmpty();

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
        #endregion

        #region Encoding / Decoding (Base64)
        // ReSharper disable StringLiteralTypo
        private const string TestRawString = "This is test example";
        private const string EncodedString = "VGhpcyBpcyB0ZXN0IGV4YW1wbGU";
        // ReSharper restore StringLiteralTypo

        [TestCase("")]
        [TestCase(" ")]
        public void Base64Encode_Empty_ReturnsExpectedString(string invalidTestString)
        {
            // Act
            string actualResult = invalidTestString.Base64Encode();

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void Base64Encode_String_ReturnsExpectedString()
        {
            // Act
            string actualResult = TestRawString.Base64Encode();

            // Assert
            Assert.That(actualResult, Is.EqualTo(EncodedString));  // Confirmed by: https://www.base64encode.org/
        }

        [Test]
        public void Base64Encode_MemoryStream_ReturnsExpectedString()
        {
            // Act
            string actualResult = new MemoryStream(Encoding.UTF8.GetBytes(TestRawString)).Base64Encode();

            // Assert
            Assert.That(actualResult, Is.EqualTo(EncodedString));  // Confirmed by: https://www.base64encode.org/
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Base64Decode_Empty_ReturnsExpectedString(string invalidTestString)
        {
            // Act
            string actualResult = invalidTestString.Base64Decode();

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void Base64Decode_ReturnsExpectedString()
        {
            // Act
            string actualResult = EncodedString.Base64Decode();

            // Assert
            Assert.That(actualResult, Is.EqualTo(TestRawString));  // Confirmed by: https://www.base64decode.org/
        }
        #endregion
    }
}