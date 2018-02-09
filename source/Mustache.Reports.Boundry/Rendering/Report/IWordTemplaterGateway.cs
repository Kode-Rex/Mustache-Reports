using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Boundry.Rendering.Report
{
    public interface IWordTemplaterGateway
    {
        RenderedDocummentOutput CreateReport(RenderWordInput input);
    }
}
