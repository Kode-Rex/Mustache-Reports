using System;
using System.IO;
using Microsoft.Extensions.Options;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Options;
using Mustache.Reports.Boundary.Pdf;
using Mustache.Reports.Data.Csv;
using Mustache.Reports.Data.PdfRendering;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Presenters;
using StoneAge.Synchronous.Process.Runner;

namespace Mustache.Reports.Data
{
    public class PdfGateway : IPdfGateway
    {
        private readonly string _libreOffice;

        public PdfGateway(IOptions<MustacheReportOptions> options)
        {
            _libreOffice = options.Value.LibreOfficeLocation;
        }

        public RenderedDocumentOutput ConvertToPdf(RenderPdfInput inputMessage)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var pdfPresenter = new PropertyPresenter<string, ErrorOutput>();

                var reportPath = PersistDocxFile(inputMessage, renderDirectory);

                CovertToPdf(reportPath, renderDirectory, pdfPresenter);

                return RenderingErrors(pdfPresenter) ? ReturnErrors(pdfPresenter) : ReturnPdf(reportPath);
            }
        }

        private void CovertToPdf(string reportPath, DisposableWorkSpace renderDirectory, PropertyPresenter<string, ErrorOutput> pdfPresenter)
        {
            var executor = new SynchronousAction(new DocxToPdfTask(_libreOffice, reportPath, renderDirectory.TmpPath), 
                new ProcessFactory());
            executor.Execute(pdfPresenter);
        }

        private bool RenderingErrors(PropertyPresenter<string, ErrorOutput> presenter)
        {
            return presenter.IsErrorResponse();
        }

        private RenderedDocumentOutput ReturnPdf(string reportPath)
        {
            var pdfPath = reportPath.Replace(".docx", ".pdf");
            var result = new RenderedDocumentOutput
            {
                Base64String = Convert.ToBase64String(File.ReadAllBytes(pdfPath)),
                ContentType = ContentTypes.Pdf
            };
            return result;
        }

        private string PersistDocxFile(RenderPdfInput inputMessage, DisposableWorkSpace renderDirectory)
        {
            var reportPath = Path.Combine(renderDirectory.TmpPath, "report.docx");
            WriteTo(reportPath, inputMessage.Base64DocxReport);
            return reportPath;
        }

        private RenderedDocumentOutput ReturnErrors(PropertyPresenter<string, ErrorOutput> presenter)
        {
            var result = new RenderedDocumentOutput();
            result.ErrorMessages.AddRange(presenter.ErrorContent.Errors);
            return result;
        }

        private void WriteTo(string filePath, string data)
        {
            var decodedData = Convert.FromBase64String(data);
            File.WriteAllBytes(filePath, decodedData);
        }

        private DisposableWorkSpace GetWorkspace()
        {
            return new DisposableWorkSpace();
        }
    }
}
