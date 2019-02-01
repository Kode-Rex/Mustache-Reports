using Mustache.Reports.Boundary.Pdf;
using Xunit;

namespace Mustache.Reports.Boundary.Tests.Pdf
{
    public class RenderPdfInputTests
    {
        [Fact]
        public void Ctor_ShouldSetFileNamePropertyToDefault()
        {
            // arrange
            // act
            var actual = new RenderPdfInput();
            // assert
            Assert.Equal("Report", actual.FileName);
        }
    }
}
