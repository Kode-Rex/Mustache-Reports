using Mustache.Reports.Boundry.Pdf;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Mustache.Reports.Boundry.Tests.Pdf
{
    public class RenderPdfInputTests
    {
        [Fact]
        public void Ctor_ShouldSetFileNamePropertyToDefault()
        {
            // arrange
            // act
            var actual = new RenderPdfInput();
            // assert
            Assert.Equal("Report", actual.FileName);
        }
    }
}
