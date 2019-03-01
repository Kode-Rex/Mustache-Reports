using System;
using System.Collections.Generic;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Domain.Pdf;
using NSubstitute;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.CleanArchitecture.Domain.Presenters;
using Xunit;

namespace Mustache.Reports.Domain.Tests
{
    public class RenderWordToPdfUseCaseTest
    {
        [Fact]
        public void Ctor_WhenNullPdfGateWay_ThrowsArgumentNullException()
        {
            //---------------Arrange-------------------
            //---------------Act----------------------
            var actual = Assert.Throws<ArgumentNullException>(() => new RenderWordToPdfUseCase(null));
            //---------------Assert-----------------------
            var expected = "pdfGateway";
            Assert.Equal(expected, actual.ParamName);
        }

        [Fact]
        public void Execute_WhenValidInputTo_ShouldRespondWithPdfContentType()
        {
            //---------------Arrange-------------------
            var gatewayResult = new RenderedDocumentOutput {Base64String = "eA=="};
            var pdfGateway = Create_Report_Gateway(gatewayResult);
           
            var input = new RenderPdfInput {Base64DocxReport = "cHVzc3k=", FileName = "report.docx"};
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.Equal("application/pdf", presenter.SuccessContent.ContentType);
        }

        [Fact]
        public void Execute_WhenRenderErrors_ShouldRespondWithErrors()
        {
            //---------------Arrange-------------------
            var gatewayResult = new RenderedDocumentOutput {ErrorMessages = new List<string> {"error"}};
            var pdfGateway = Create_Report_Gateway(gatewayResult);
            
            var input = new RenderPdfInput { Base64DocxReport = "cHVzc3k=", FileName = "report.docx" };
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.True(presenter.IsErrorResponse());
        }

        private static IPdfGateway Create_Report_Gateway(RenderedDocumentOutput gatewayResult)
        {
            var gateway = Substitute.For<IPdfGateway>();
            gateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(gatewayResult);
            return gateway;
        }
    }
}
