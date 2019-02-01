using Mustache.Reports.Boundary.Report.Word;
using Xunit;

namespace Mustache.Reports.Boundary.Tests.Word
{
    public class RenderWordInputTests
    {
        [Fact]
        public void Ctor_ShouldSetFileNamePropertyToDefault()
        {
            // arrange
            // act
            var actual = new RenderWordInput();
            // assert
            Assert.Equal("report.docx", actual.ReportName);
        }
    }
}
