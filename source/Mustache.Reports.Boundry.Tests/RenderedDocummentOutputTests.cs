using System.IO;
using System.Text;
using Xunit;

namespace Mustache.Reports.Boundry.Tests
{
    public class RenderedDocummentOutputTests
    {
        [Fact]
        public void Ctor_ShouldInitalizeErrorList()
        {
            //---------------Arrange------------------
            //---------------Act----------------------
            var renderedDocumentOutput = new RenderedDocummentOutput();
            //---------------Assert-------------------
            Assert.NotNull(renderedDocumentOutput.ErrorMessages);
        }

        [Fact]
        public void HasErrors_WhenNoErrors_ShouldReturnFalse()
        {
            //---------------Arrange------------------
            //---------------Act----------------------
            var renderedDocumentOutput = new RenderedDocummentOutput();
            //---------------Assert-------------------
            Assert.False(renderedDocumentOutput.HasErrors());
        }

        [Fact]
        public void HasErrors_Errors_ShouldReturnTrue()
        {
            //---------------Arrange------------------
            var renderedDocumentOutput = new RenderedDocummentOutput();
            //---------------Act----------------------
            renderedDocumentOutput.ErrorMessages.Add("error 1");
            //---------------Assert-------------------
            Assert.True(renderedDocumentOutput.HasErrors());
        }

        [Fact]
        public void FetchDocumetnAsByteArray_WhenNullStream_ShouldReturnEmptyByteArray()
        {
            //---------------Arrange------------------
            var renderedDocumentOutput = new RenderedDocummentOutput();
            //---------------Act----------------------
            //---------------Assert-------------------
            Assert.Empty(renderedDocumentOutput.FetchDocumentAsByteArray());
        }

        [Fact]
        public void FetchDocumetnAsByteArray_WhenValidStream_ShouldReturnByteArray()
        {
            //---------------Arrange------------------
            var inputString = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            using (var stream = new MemoryStream(inputBytes))
            {
                //---------------Act----------------------
                var renderedDocumentOutput = new RenderedDocummentOutput
                {
                    DocumentStream = stream
                };

                //---------------Assert-------------------
                Assert.Equal(inputBytes, renderedDocumentOutput.FetchDocumentAsByteArray());
            }
        }
    }
}
