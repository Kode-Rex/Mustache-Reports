using System;
using System.IO;
using Microsoft.Extensions.Options;
using Mustache.Reports.Boundary;
using Mustache.Reports.Boundary.Csv;
using Mustache.Reports.Boundary.Options;
using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Presenters;
using StoneAge.Synchronous.Process.Runner;

namespace Mustache.Reports.Data.Csv
{
    public class CsvGateway : ICsvGateway
    {
        private readonly string _libreOffice;

        public CsvGateway(IOptions<MustacheReportOptions> options)
        {
            _libreOffice = options.Value.LibreOfficeLocation;
        }

        public RenderedDocumentOutput ConvertToCsv(RenderCsvInput inputMessage)
        {
            using (var renderDirectory = GetWorkspace())
            {
                var pdfPresenter = new PropertyPresenter<string, ErrorOutput>();

                var reportPath = PresistFile(inputMessage, renderDirectory);

                CovertToCsv(reportPath, renderDirectory, pdfPresenter);

                return RenderingErrors(pdfPresenter) ? ReturnErrors(pdfPresenter) : ReturnPdf(reportPath);
            }
        }

        private void CovertToCsv(string reportPath, DisposableWorkSpace renderDirectory, PropertyPresenter<string, ErrorOutput> pdfPresenter)
        {
            var executor = new SynchronousAction(new XlsxToCsvTask(_libreOffice, reportPath, renderDirectory.TmpPath),
                new ProcessFactory());
            executor.Execute(pdfPresenter);
        }

        private bool RenderingErrors(PropertyPresenter<string, ErrorOutput> presenter)
        {
            return presenter.IsErrorResponse();
        }

        private RenderedDocumentOutput ReturnPdf(string reportPath)
        {
            var filePath = reportPath.Replace(".xlsx", ".csv");
            var result = new RenderedDocumentOutput
            {
                Base64String = Convert.ToBase64String(File.ReadAllBytes(filePath)),
                ContentType = ContentTypes.Pdf
            };
            return result;
        }

        private string PresistFile(RenderCsvInput inputMessage, DisposableWorkSpace renderDirectory)
        {
            var reportPath = Path.Combine(renderDirectory.TmpPath, "report.xlsx");
            WriteTo(reportPath, inputMessage.Base64ExcelFile);
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
