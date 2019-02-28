using System;
using System.Collections.Generic;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Pdf;
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
            var pdfGateway = Substitute.For<IPdfGateway>();
            pdfGateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(new RenderedDocumentOutput{Base64String = "eA==" });
            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            var input = new RenderPdfInput {Base64DocxReport = "cHVzc3k=", FileName = "report.docx"};
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.Equal("application/pdf", presenter.SuccessContent.ContentType);
        }

        [Fact]
        public void Execute_WhenRenderErrors_ShouldRespondWithErrors()
        {
            //---------------Arrange-------------------
            var pdfGateway = Substitute.For<IPdfGateway>();
            pdfGateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(new RenderedDocumentOutput { ErrorMessages = new List<string>{"error"}});
            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            var input = new RenderPdfInput { Base64DocxReport = "cHVzc3k=", FileName = "report.docx" };
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.True(presenter.IsErrorResponse());
        }
    }
}
