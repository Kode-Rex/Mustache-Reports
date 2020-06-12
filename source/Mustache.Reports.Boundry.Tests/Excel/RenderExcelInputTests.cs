using Mustache.Reports.Boundary.Report.Excel;
using System.Collections.Generic;
using Xunit;

namespace Mustache.Reports.Boundary.Tests.Excel
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
            Assert.Equal(new List<int> { 1 }, actual.SheetNumbers);
        }

        [Fact]
        public void When_Setting_Sheet_Number_Should_Convert_To_Sheet_Numbers()
        {
            // arrange
            // act
            var actual = new RenderExcelInput
            {
                SheetNumber = 9
            };
            // assert
            Assert.Equal(new List<int> { 9 }, actual.SheetNumbers);
        }
    }
}
