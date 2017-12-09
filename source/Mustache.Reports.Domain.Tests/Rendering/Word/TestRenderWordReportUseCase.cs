using System;
using Mustache.Reports.Boundary.Rendering.Word;
using Mustache.Reports.Domain.Rendering.Word;
using Mustache.Reports.Domain.TestData.Rendering.Word;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Domain.Tests.Rendering.Word
{
    [TestFixture]
    public class TestRenderWordReportUseCase
    {
        [TestFixture]
        public class ValidInput
        {
            [Test]
            public void Should_UseTemplaterGateway_ToRenderReport_WithTemplateFromInput()
            {
                // Arrange
                var input = RenderReportInputTestDataBuilder
                    .Create()
                    .WithValidTemplateAndData()
                    .Build();

                InMemoryWordInputFile actualTemplate = null;
                var useCase = CreateRenderWordReportUseCase(t => actualTemplate = t);
                // Act
                useCase.Execute(input, CreatePropertyPresenter());
                // Assert
                Expectations.Expect(actualTemplate).To.Have.ContentsEqualTo(input.Template);
            }

            [Test]
            public void Should_UseTemplaterGateway_ToRenderReport_WithDataFromInput()
            {
                // Arrange
                var input = RenderReportInputTestDataBuilder
                    .Create()
                    .WithValidTemplateAndData()
                    .Build();

                object actualData = null;
                var useCase = CreateRenderWordReportUseCase(captureData: t => actualData = t);
                // Act
                useCase.Execute(input, CreatePropertyPresenter());
                // Assert
                Expectations.Expect(actualData).To.Be(input.Data);
            }
        }

        [TestFixture]
        public class SuccessfulRender
        {
            [Test]
            public void Should_OutputFileFromTemplaterGateway()
            {
                // Arrange
                var input = RenderReportInputTestDataBuilder
                    .Create()
                    .WithValidTemplateAndData()
                    .Build();
                var output = CreatePropertyPresenter();

                var expectedFile = Substitute.For<IWordFileOutput>();

                var useCase = CreateRenderWordReportUseCase(expectedFile);
                // Act
                useCase.Execute(input, output);
                // Assert
                Expectations.Expect(output.SuccessContent).To.Be(expectedFile);
            }
        }

        // TODO deal error(s) from templater, should we handle this using exceptions or something that wraps the current IWordFileOutput of the gateway?
        // TODO invalid / missing template
        // TODO non base64 dataUri
        // TODO should the use case check if the template is not a docx file? Or perhapse the InMemoryWordInputFile should.
        // TODO I'm guessing at some point we need to care about the name of the output file

        private static RenderWordReportUseCase CreateRenderWordReportUseCase(IFileOutput gatewayOutput)
        {
            var templaterGateway = Substitute.For<IWordTemplaterGateway>();
            templaterGateway
                .Render(Arg.Any<IWordFileInput>(), Arg.Any<object>())
                .Returns(gatewayOutput);

            return new RenderWordReportUseCase(
                templaterGateway
            );
        }

        private static RenderWordReportUseCase CreateRenderWordReportUseCase(
            Action<InMemoryWordInputFile> captureTemplate = null,
            Action<object> captureData = null
        )
        {
            var templaterGateway = Substitute.For<IWordTemplaterGateway>();
            templaterGateway
                .Render(
                    Arg.Do(captureTemplate ?? Noop<object>()),
                    Arg.Do(captureData ?? Noop<object>())
                );

            return new RenderWordReportUseCase(
                templaterGateway
            );
        }

        private static Action<T> Noop<T>()
        {
            return o => { };
        }

        private static PropertyPresenter<IWordFileOutput, ErrorOutput> CreatePropertyPresenter()
        {
            return new PropertyPresenter<IWordFileOutput, ErrorOutput>();
        }
    }
}
