using System;
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

        //[Fact]
        //public void Execute_WhenValidInputTo_ShouldRespondWithSuccess()
        //{
        //    //---------------Arrange-------------------
        //    var pdfGateway = Substitute.For<IPdfGateway>();
        //    var usecase = new RenderWordToPdfUseCase(pdfGateway);
        //    var input = new RenderPdfInput();
        //    var presenter = new PropertyPresenter<PdfFileOutput, ErrorOutput>();
        //    //---------------Act----------------------
        //    usecase.Execute(input, presenter);
        //    //---------------Assert-----------------------
            
        //}
    }
}
