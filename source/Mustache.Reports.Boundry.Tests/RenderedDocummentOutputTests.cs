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
            var renderedDocumentOutput = new RenderedDocumentOutput();
            //---------------Assert-------------------
            Assert.NotNull(renderedDocumentOutput.ErrorMessages);
        }

        [Fact]
        public void HasErrors_WhenNoErrors_ShouldReturnFalse()
        {
            //---------------Arrange------------------
            //---------------Act----------------------
            var renderedDocumentOutput = new RenderedDocumentOutput();
            //---------------Assert-------------------
            Assert.False(renderedDocumentOutput.HasErrors());
        }

        [Fact]
        public void HasErrors_Errors_ShouldReturnTrue()
        {
            //---------------Arrange------------------
            var renderedDocumentOutput = new RenderedDocumentOutput();
            //---------------Act----------------------
            renderedDocumentOutput.ErrorMessages.Add("error 1");
            //---------------Assert-------------------
            Assert.True(renderedDocumentOutput.HasErrors());
        }

        [Fact]
        public void FetchDocumetnAsByteArray_WhenNullBase64String_ShouldReturnEmptyByteArray()
        {
            //---------------Arrange------------------
            var renderedDocumentOutput = new RenderedDocumentOutput();
            //---------------Act----------------------
            //---------------Assert-------------------
            Assert.Empty(renderedDocumentOutput.FetchDocumentAsByteArray());
        }

        [Fact]
        public void FetchDocumetnAsByteArray_WhenValidBase64String_ShouldReturnByteArray()
        {
            //---------------Arrange------------------
            var renderedDocumentOutput = new RenderedDocumentOutput
            {
                //---------------Act----------------------
                Base64String = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"
            };
            //---------------Assert-------------------
            Assert.NotEmpty(renderedDocumentOutput.FetchDocumentAsByteArray());
        }
    }
}
