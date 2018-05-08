using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Boundry.Report.Excel;
using Mustache.Reports.Boundry.Report.Word;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mustache.Reports.Boundry.Tests.Word
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
