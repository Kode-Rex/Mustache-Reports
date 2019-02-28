using System;
using System.Collections.Generic;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Report;
using Mustache.Reports.Boundary.Report.Word;
using NSubstitute;
using Xunit;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;
using StoneAge.CleanArchitecture.Domain.Presenters;

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
            var gatewayResult = new RenderedDocumentOutput {Base64String = "eA=="};
            var gateway = Create_Report_Gateway(gatewayResult);

            var input = new RenderWordInput {JsonModel = "", ReportName = "Test.docx", TemplateName = "Test"};
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var usecase = new RenderWordUseCase(gateway);
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
            var gatewayResult = new RenderedDocumentOutput { ErrorMessages = new List<string> { "error" }};
            var gateway = Create_Report_Gateway(gatewayResult);
            
            var input = new RenderWordInput { JsonModel = "", ReportName = "Test.docx", TemplateName = "Test" };
            var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();

            var usecase = new RenderWordUseCase(gateway);
            //---------------Act----------------------
            usecase.Execute(input, presenter);
            //---------------Assert-----------------------
            Assert.True(presenter.IsErrorResponse());
        }

        private static IReportGateway Create_Report_Gateway(RenderedDocumentOutput gatewayResult)
        {
            var gateway = Substitute.For<IReportGateway>();
            gateway.CreateWordReport(Arg.Any<RenderWordInput>()).Returns(gatewayResult);
            return gateway;
        }
    }
}
