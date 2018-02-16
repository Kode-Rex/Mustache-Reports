using System;
using System.Collections.Generic;
using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Pdf;
using Mustache.Reports.Boundry.Report.Word;
using NSubstitute;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using Xunit;

namespace Mustache.Reports.Domain.Tests
{
    public class RenderWordUseCaseTest
    {
        [Fact]
        public void Ctor_WhenNullWordGateWay_ThrowsArgumentNullException()
        {
            //---------------Arrange-------------------
            //---------------Act----------------------
            var actual = Assert.Throws<ArgumentNullException>(() => new RenderWordUseCase(null));
            //---------------Assert-----------------------
            var expected = "wordGateway";
            Assert.Equal(expected, actual.ParamName);
        }

        [Fact]
        public void Execute_WhenValidInputTo_ShouldRespondWithWordContentType()
        {
            //---------------Arrange-------------------
            var gateway = Substitute.For<IWordGateway>();
            gateway.CreateReport(Arg.Any<RenderWordInput>()).Returns(new RenderedDocummentOutput{Base64String = "eA==" });
            var usecase = new RenderWordUseCase(gateway);
            var input = new RenderWordInput {JsonModel = "", ReportName = "Test.docx", TemplateName = "Test"};
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            var expected = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            Assert.Equal(expected, presenter.SuccessContent.ContentType);
        }

        [Fact]
        public void Execute_WhenRenderErrors_ShouldRespondWithErrors()
        {
            //---------------Arrange-------------------
            var gateway = Substitute.For<IWordGateway>();
            gateway.CreateReport(Arg.Any<RenderWordInput>()).Returns(new RenderedDocummentOutput { ErrorMessages = new List<string>{"error"}});
            var usecase = new RenderWordUseCase(gateway);
            var input = new RenderWordInput { JsonModel = "", ReportName = "Test.docx", TemplateName = "Test" };
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.True(presenter.IsErrorResponse());
        }
    }
}
