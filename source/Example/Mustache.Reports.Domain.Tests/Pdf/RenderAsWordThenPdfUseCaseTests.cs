using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Word;
using Mustache.Reports.Domain.Pdf;
using Mustache.Reports.Domain.Word;
using NSubstitute;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.CleanArchitecture.Domain.Presenters;
using Xunit;

namespace Mustache.Reports.Domain.Tests.Pdf
{
    public class RenderAsWordThenPdfUseCaseTests
    {
        [Fact]
        public void Execute_WhenValidInputTo_ShouldRespondWithPdfContentType()
        {
            //---------------Arrange-------------------
            var input = new RenderWordInput { JsonModel = "{}", ReportName = "Test.docx", TemplateName = "Test" };
            var reportResult = new RenderedDocumentOutput {Base64String = "", ContentType = ContentTypes.Word};
            var pdfResult = new RenderedDocumentOutput {Base64String = "", ContentType = ContentTypes.Pdf};

            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var reportGateway = Create_Report_Gateway(reportResult);
            var pdfGateway = Create_Pdf_Gateway(pdfResult);

            var wordUsecase = new RenderWordUseCase(reportGateway);
            var pdfUsecase = new RenderWordToPdfUseCase(pdfGateway);
            var sut = new RenderAsWordThenPdfUseCase(wordUsecase, pdfUsecase);
            //---------------Act-----------------------
            sut.Execute(input, presenter);
            //---------------Assert--------------------
            Assert.Equal("application/pdf",presenter.SuccessContent.ContentType);
        }

        private static IReportGateway Create_Report_Gateway(RenderedDocumentOutput gatewayResult)
        {
            var gateway = Substitute.For<IReportGateway>();
            gateway.CreateWordReport(Arg.Any<RenderWordInput>()).Returns(gatewayResult);
            return gateway;
        }

        private static IPdfGateway Create_Pdf_Gateway(RenderedDocumentOutput gatewayResult)
        {
            var gateway = Substitute.For<IPdfGateway>();
            gateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(gatewayResult);
            return gateway;
        }
    }
}
