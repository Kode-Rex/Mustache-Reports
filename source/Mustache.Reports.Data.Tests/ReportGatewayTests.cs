using System.IO;
using Microsoft.Extensions.Configuration;
using Mustache.Reports.Boundry.Report.Word;
using Xunit;
using Mustache.Reports.Boundry.Report.Excel;

namespace Mustache.Reports.Data.Tests
{
    public class ReportGatewayTests
    {
        [Fact]
        public void CreateWordReport_WhenValidInput_ShouldReturnRenderedReport()
        {
            //---------------Arrange------------------
            var configuration = SetupConfiguration();
            var reportData = File.ReadAllText(configuration["Reporting:RelativeSampleDataLocation"]);
            var wordGateway = new ReportGateway(configuration);
            var input = new RenderWordInput {JsonModel = reportData, ReportName = "test.docx", TemplateName = "ReportWithImages" };
            //---------------Act----------------------
            var actual = wordGateway.CreateWordReport(input);
            //---------------Assert-------------------
            var expected = File.ReadAllText("Expected\\RenderedWordBase64.txt");
            Assert.Equal(expected.Substring(0,50), actual.Base64String.Substring(0,50));
        }

        [Fact]
        public void CreateWordReport_WhenInvalidTemplateName_ShouldReturnTemplateNameError()
        {
            //---------------Arrange------------------
            var configuration = SetupConfiguration();
            var wordGateway = new ReportGateway(configuration);
            var input = new RenderWordInput { JsonModel = "", ReportName = "test.docx", TemplateName = "INVALID_NAME" };
            //---------------Act----------------------
            var actual = wordGateway.CreateWordReport(input);
            //---------------Assert-------------------
            Assert.True(actual.HasErrors());
            Assert.Equal("Invalid Report Type [INVALID_NAME]", actual.ErrorMessages[0]);
        }

        [Fact]
        public void CreateExcelReport_WhenValidInput_ShouldReturnRenderedReport()
        {
            //---------------Arrange------------------
            var configuration = SetupConfiguration();
            var reportData = File.ReadAllText(configuration["Reporting:RelativeSampleDataLocation"]);
            var wordGateway = new ReportGateway(configuration);
            var input = new RenderExcelInput { JsonModel = reportData, ReportName = "test.xslx", TemplateName = "SimpleReport" };
            //---------------Act----------------------
            var actual = wordGateway.CreateExcelReport(input);
            //---------------Assert-------------------
            var expected = File.ReadAllText("Expected\\RenderedWordBase64.txt");
            Assert.Equal(expected.Substring(0, 50), actual.Base64String.Substring(0, 50));
        }

        [Fact]
        public void CreateExcelReport_WhenInvalidTemplateName_ShouldReturnTemplateNameError()
        {
            //---------------Arrange------------------
            var configuration = SetupConfiguration();
            var wordGateway = new ReportGateway(configuration);
            var input = new RenderExcelInput { JsonModel = "", ReportName = "test.xslx", TemplateName = "INVALID_NAME" };
            //---------------Act----------------------
            var actual = wordGateway.CreateExcelReport(input);
            //---------------Assert-------------------
            Assert.True(actual.HasErrors());
            Assert.Equal("Invalid Report Type [INVALID_NAME]", actual.ErrorMessages[0]);
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}
