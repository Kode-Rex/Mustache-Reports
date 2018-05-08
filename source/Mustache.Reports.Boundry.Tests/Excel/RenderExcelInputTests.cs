using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Boundry.Report.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mustache.Reports.Boundry.Tests.Excel
{
    public class RenderExcelInputTests
    {
        [Fact]
        public void Ctor_ShouldSetFileNamePropertyToDefault()
        {
            // arrange
            // act
            var actual = new RenderExcelInput();
            // assert
            Assert.Equal("report.xlsx", actual.ReportName);
        }

        [Fact]
        public void Ctor_ShouldSetSheetNumberToOne()
        {
            // arrange
            // act
            var actual = new RenderExcelInput();
            // assert
            Assert.Equal(1, actual.SheetNumber);
        }
    }
}
