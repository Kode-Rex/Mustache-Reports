using System;
using Mustache.Reports.Boundary.Rendering;
using PeanutButter.RandomGenerators;

namespace Mustache.Reports.Domain.TestData.Rendering.Word
{
    public class RenderReportInputTestDataBuilder : GenericBuilder<RenderReportInputTestDataBuilder, RenderReportInput>
    {
        public RenderReportInputTestDataBuilder WithValidTemplateAndData()
        {
            // TODO it would be nice if this was actually a valid word file.
            var base64String = Convert.ToBase64String(RandomValueGen.GetRandomBytes(512));
            return WithProp(o => o.Template = $"data:text/plain;base64,{base64String}")
                .WithProp(o => o.Data = new object());
        }
    }
}