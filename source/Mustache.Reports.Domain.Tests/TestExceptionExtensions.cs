using System.Collections.Generic;
using Mustache.Reports.Domain.TestData.Errors;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace Mustache.Reports.Domain.Tests
{
    [TestFixture]
    public class TestExceptionExtensions
    {
        [TestFixture]
        public class ManyNestedExceptions
        {
            [Test]
            public void Should_ReturnMessageAndAllInnerExceptionMessages()
            {
                // Arrange
                var exception = ExceptionTestDataBuilder.Create()
                    .WithRandomMessage()
                    .WithNestedInnerExceptions(2)
                    .Build();

                var expected = new List<string>
                {
                    exception.Message,
                    exception.InnerException.Message,
                    exception.InnerException.InnerException.Message
                };
                // Act
                var actual = exception.AllMessages();
                // Assert
                Expect(actual).To.Be.Equivalent.To(expected);
            }
        }

        [TestFixture]
        public class NoNestedExceptions
        {
            [Test]
            public void Should_ReturnMessageAndAllInnerExceptionMessages()
            {
                // Arrange
                var exception = ExceptionTestDataBuilder.Create()
                    .WithRandomMessage()
                    .Build();

                var expected = new List<string>
                {
                    exception.Message,
                };
                // Act
                var actual = exception.AllMessages();
                // Assert
                Expect(actual).To.Be.Equivalent.To(expected);
            }
        }
    }
}