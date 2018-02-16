using System;
using System.Collections.Generic;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Pdf;
using NSubstitute;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
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
            pdfGateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(new RenderedDocummentOutput{Base64String = "eA==" });
            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            var input = new RenderPdfInput {Base64DocxReport = "cHVzc3k=", FileName = "report.docx"};
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
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
            pdfGateway.ConvertToPdf(Arg.Any<RenderPdfInput>()).Returns(new RenderedDocummentOutput { ErrorMessages = new List<string>{"error"}});
            var usecase = new RenderWordToPdfUseCase(pdfGateway);
            var input = new RenderPdfInput { Base64DocxReport = "cHVzc3k=", FileName = "report.docx" };
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.True(presenter.IsErrorResponse());
        }
    }
}
