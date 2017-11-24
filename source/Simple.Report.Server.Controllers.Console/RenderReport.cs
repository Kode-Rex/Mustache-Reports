using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Simple.Report.Server.Boundry.ReportRendering;
using Simple.Report.Server.Data.ReportRendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Simple.Report.Server.Controllers.Console
{
    public class RenderReport
    {
        private readonly IRenderReportUseCase _usecase;
        private readonly ILogger _logger;

        public RenderReport(IRenderReportUseCase usecase, ILoggerFactory logFactory)
        {
            _usecase = usecase;
            _logger = logFactory.CreateLogger<RenderReport>();
        }

        public void Run(string libreOfficeLocation, string reportOutputDirectory, string reportDataFilePath)
        {
            RenderReportWithImages(libreOfficeLocation, reportOutputDirectory, reportDataFilePath);
        }

        private void RenderReportWithImages(string libreOfficeLocation,  string reportOuputDirectory, string reportDataFilePath)
        {
            var jsonData = File.ReadAllText(reportDataFilePath);

            var inputMessage = new RenderReportInputMessage
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };

            var docxPresenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            _usecase.Execute(inputMessage, docxPresenter);

            if (docxPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(docxPresenter);
                return;
            }

            var successContent = docxPresenter.SuccessContent;
            var reportPath = PersistReport(reportOuputDirectory, successContent);

            var pdfPresenter = RenderReportToPdf(libreOfficeLocation, reportOuputDirectory, reportPath);
            if (pdfPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(pdfPresenter);
                return;
            }
            
            _logger.LogInformation($"Report output to directory [ {reportOuputDirectory} ]");
            _logger.LogInformation("");
            _logger.LogInformation("Press enter to exit.");
            System.Console.ReadLine();
        }

        private PropertyPresenter<string, ErrorOutputMessage> RenderReportToPdf(string libreOfficeLocation, string reportOuputDirectory, string reportPath)
        {
            var pdfPresenter = new PropertyPresenter<string, ErrorOutputMessage>();
            var executor = new SynchronousAction(new ConvertDocxToPdfTask(libreOfficeLocation, reportPath, reportOuputDirectory),
                new ProcessFactory());
            executor.Execute(pdfPresenter);
            return pdfPresenter;
        }

        private void WriteErrorsToConsole<T>(PropertyPresenter<T, ErrorOutputMessage> presenter)
        {
            _logger.LogError("The following errors occured: ");
            _logger.LogError(string.Join(", ", (IEnumerable<string>)presenter.ErrorContent.Errors));
            _logger.LogError("");
            _logger.LogError("Press enter to exit.");
            System.Console.ReadLine();
        }

        private string PersistReport(string reportOuputDirectory, IFileOutput successContent)
        {
            EnsureDirectory(reportOuputDirectory);

            var reportPath = Path.Combine(reportOuputDirectory, $"{successContent.FileName}.docx");
            RemoveOldReport(reportPath);

            WriteReport(successContent, reportPath);

            return reportPath;
        }

        private void WriteReport(IFileOutput successContent, string reportPath)
        {
            using (var stream = successContent.GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    var reportBytes = memoryStream.ToArray();
                    File.WriteAllBytes(reportPath, reportBytes);
                }
            }
        }

        private void RemoveOldReport(string reportPath)
        {
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }
        }

        private void EnsureDirectory(string reportOuputDirectory)
        {
            if (!Directory.Exists(reportOuputDirectory))
            {
                Directory.CreateDirectory(reportOuputDirectory);
            }
        }
    }
}
