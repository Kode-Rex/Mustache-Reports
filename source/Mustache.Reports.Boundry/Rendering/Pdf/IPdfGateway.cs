using System;
using System.Collections.Generic;
using System.Text;

namespace Mustache.Reports.Boundry.Rendering.Pdf
{
    public interface IPdfGateway
    {
        RenderedDocummentOutput ConvertToPdf(RenderPdfInput inputMessage);
    }
}
