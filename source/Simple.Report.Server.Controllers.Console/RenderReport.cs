using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        public RenderReport(IRenderReportUseCase usecase)
        {
            _usecase = usecase;
        }

        public void Run(string reportOutputDirectory, string reportDataFilePath)
        {
            RenderReportWithImages(reportOutputDirectory, reportDataFilePath);
        }

        private void RenderReportWithImages(string reportOuputDirectory, string reportDataFilePath)
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

            var pdfPresenter = RenderReportToPdf(reportOuputDirectory, reportPath);
            if (pdfPresenter.IsErrorResponse())
            {
                WriteErrorsToConsole(pdfPresenter);
                return;
            }

            System.Console.WriteLine($"Report output to [{pdfPresenter.SuccessContent}]");
            System.Console.WriteLine("");
            System.Console.WriteLine("Press enter to exit.");
            System.Console.ReadLine();
        }

        private PropertyPresenter<string, ErrorOutputMessage> RenderReportToPdf(string reportOuputDirectory, string reportPath)
        {
            var pdfPresenter = new PropertyPresenter<string, ErrorOutputMessage>();
            var executor = new SynchronousAction(new ConvertDocxToPdfTask(reportPath, reportOuputDirectory),
                new ProcessFactory());
            executor.Execute(pdfPresenter);
            return pdfPresenter;
        }

        private void WriteErrorsToConsole<T>(PropertyPresenter<T, ErrorOutputMessage> presenter)
        {
            System.Console.WriteLine("The following errors occured: ");
            System.Console.WriteLine(string.Join(", ", (IEnumerable<string>)presenter.ErrorContent.Errors));
            System.Console.WriteLine("");
            System.Console.WriteLine("Press enter to exit.");
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
