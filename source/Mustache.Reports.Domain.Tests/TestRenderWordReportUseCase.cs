using Mustache.Reports.Boundary.Rendering;
using Mustache.Reports.Domain.Rendering;
using NExpect;
using NUnit.Framework;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using static NExpect.Expectations;

namespace Mustache.Reports.Domain.Tests
{
    [TestFixture]
    public class TestRenderWordReportUseCase
    {
        [TestFixture]
        public class ValidInput
        {
            [Test]
            public void Should_OutputWordFile()
            {
                // Arrange
                var input = new RenderReportInput();
                var useCase = new RenderWordReportUseCase();
                var presenter = new PropertyPresenter<IFileOutput, ErrorOutput>();
                // Act
                useCase.Execute(input, presenter);
                // Assert
                Expect(presenter.SuccessContent).To.Be.An.Instance.Of<WordFileOutput>();
            }

            // TODO decide on what is going to be on the input
            // TODO call though to the rendering gateway (the thing that will execute the node app)
            // TODO when calling throuh to the gateway consider using something similar to the WordOutputFile 
            //      except it would be a WordInputFile, the use case would translate what ever format the 
            //      incoming tempate data is in into this before sending it to the gateway
            // TODO submit bug or do PR for "Expect(new Object().GetType()).To.Equal(typeof(Object));" hanging NExpect
        }
    }
}
