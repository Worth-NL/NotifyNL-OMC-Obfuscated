// © 2024, Worth Systems.

using EventsHandler.Extensions;
using System.Text;

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
        private const string EncodedString = "VGhpcyBpcyB0ZXN0IGV4YW1wbGU=";
        // ReSharper restore StringLiteralTypo

        [Test]
        public void Base64Encode_Empty_ReturnsExpectedString()
        {
            // Act
            string actualResult = Encoding.UTF8.GetBytes(string.Empty).Base64Encode();

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void Base64Encode_String_ReturnsExpectedString()
        {
            // Act
            string actualResult = Encoding.UTF8.GetBytes(TestRawString).Base64Encode();

            // Assert
            Assert.That(actualResult, Is.EqualTo(EncodedString));  // Confirmed by: https://www.base64encode.org/
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Base64Decode_Empty_ReturnsExpectedString(string invalidTestString)
        {
            // Act
            string actualResult = Encoding.UTF8.GetString(invalidTestString.Base64Decode());

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public void Base64Decode_ReturnsExpectedString()
        {
            // Act
            string actualResult = Encoding.UTF8.GetString(EncodedString.Base64Decode());

            // Assert
            Assert.That(actualResult, Is.EqualTo(TestRawString));  // Confirmed by: https://www.base64decode.org/
        }
        #endregion

        #region Compressing / Decompressing
        // ReSharper disable StringLiteralTypo
        private const string TestReferenceString =
            "{" +
              "\"Notification\":{" +
                "\"actie\":\"create\"," +
                "\"kanaal\":\"zaken\"," +
                "\"resource\":\"status\"," +
                "\"kenmerken\":{" +
                  "\"zaaktype\":\"https://lb.zgw.sandbox-marnix.csp-nijmegen.nl/open-zaak/catalogi/api/v1/zaaktypen/20f1bdac-217c-406c-8a48-0661e8a5ddc2\"," +
                  "\"bronorganisatie\":\"001479179\"," +
                  "\"vertrouwelijkheidaanduiding\":\"-\"," +
                  "\"objectType\":\"http://0.0.0.0:0/\"," +
                  "\"besluittype\":\"http://0.0.0.0:0/\"," +
                  "\"verantwoordelijkeOrganisatie\":\"\"" +
                "}," +
                "\"hoofdObject\":\"https://lb.zgw.sandbox-marnix.csp-nijmegen.nl/open-zaak/zaken/api/v1/zaken/682900a9-4031-480f-9734-e6dff9efd2ec\"," +
                "\"resourceUrl\":\"https://lb.zgw.sandbox-marnix.csp-nijmegen.nl/open-zaak/zaken/api/v1/statussen/a04592bf-663f-4592-8be2-a9ae1d066324\"," +
                "\"aanmaakdatum\":\"2024-09-19T12:52:39.7400000\\u002B00:00\"" +
              "}," +
              "\"CaseId\":\"682900a9-4031-480f-9734-e6dff9efd2ec\"," +
              "\"PartyId\":\"dfbf6cf9-1feb-4f11-98a1-09d80e122bf2\"" +
            "}";

        private const string CompressedEncodedString = "eJytUrFy3CAQ/RfVRgKkk4TKpHJjp3C6NCtY7riTQIOQzz6P/z2LMhknRWYyk0ADy9t9b9/yVjyE5KzTkFzwxfBWgE4Oi" +
                                                       "6HQESFhcVdcwANMFLnBBT0FIq5hizqD1gRpWzMI/Ywxv1OJG8AlvS4ZcEppWYeqmsbydryWK3gzhhc2Q/TupdTrwrw7z3" +
                                                       "hEX/qpCgt6lrMr0gNTOLoKFlc9i+pnSV9JbsVoQDMpOs0a3mrWQ9Mz3rYCezgYoyUJGmPwIR7Bu5Vay1I4F02nRKfo9Rl" +
                                                       "jimG74uTOlxM6AyRsc8b5IyEZIcJ4Rp2ePrqgJni574FXmQDXaXMp/RlBJODTNYRodh58/E1P8X5XnEKw5nGn+gez9sF8" +
                                                       "OJUvbS8V56DIoFqwpueWqa5uGLbGWoXWSNS/jPJrnP4X/48vseYAbw5Kjpa1bW1ZPrN+RMlAAQpD86plQxrI+pmqGEqbS" +
                                                       "YTksmFcMaGehBwOcqhV2TU8r28b5/ITJ3t5Nu8zrHhvKOUvm/0CMb3uCcaOttWWSCyOrLFCMNWDIFrTcxSSRMvi/TsnZv4y";
        // ReSharper restore StringLiteralTypo

        [Test]
        public async Task CompressGZipAsync_Empty_ReturnsExpectedString()
        {
            // Act
            string actualResult = await string.Empty.CompressGZipAsync(CancellationToken.None);

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public async Task CompressGZipAsync_ReturnsExpectedString()
        {
            // Act
            string actualResult = await TestReferenceString.CompressGZipAsync(CancellationToken.None);

            // Assert
            Assert.That(actualResult, Is.EqualTo(CompressedEncodedString));
        }

        [Test]
        public async Task DecompressGZipAsync_Empty_ReturnsExpectedString()
        {
            // Act
            string actualResult = await string.Empty.DecompressGZipAsync(CancellationToken.None);

            // Assert
            Assert.That(actualResult, Is.Empty);
        }

        [Test]
        public async Task DecompressGZipAsync_ReturnsExpectedString()
        {
            // Act
            string actualResult = await CompressedEncodedString.DecompressGZipAsync(CancellationToken.None);

            // Assert
            Assert.That(actualResult, Is.EqualTo(TestReferenceString));
        }
        #endregion
    }
}