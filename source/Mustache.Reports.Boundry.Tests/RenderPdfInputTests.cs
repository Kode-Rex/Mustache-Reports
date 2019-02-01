using Mustache.Reports.Boundary.Pdf;
using Xunit;

namespace Mustache.Reports.Boundary.Tests
{
    public class RenderPdfInputTests
    {
        [Fact]
        public void Ctor_WhenNoNameGiven_ShouldDefaultToReport()
        {
            //---------------Arrange-------------------
            //---------------Act----------------------
            var sut = new RenderPdfInput();
            //---------------Assert-----------------------
            var expected = "Report";
            Assert.Equal(expected, sut.FileName);
        }

        [Fact]
        public void Ctor_WhenNameGiven_ShouldUseName()
        {
            //---------------Arrange-------------------
            //---------------Act----------------------
            var sut = new RenderPdfInput {FileName = "NewReport"};
            //---------------Assert-----------------------
            var expected = "NewReport";
            Assert.Equal(expected, sut.FileName);
        }

    }
}
