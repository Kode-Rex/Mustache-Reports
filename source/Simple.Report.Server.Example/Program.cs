using System;
using System.Collections.Generic;
using System.IO;
using Simple.Report.Server.Data.Repositories;
using Simple.Report.Server.Data.Task;
using Simple.Report.Server.Domain.Messages.Input;
using Simple.Report.Server.Domain.UseCases;
using Simple.Report.Server.UseCase;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;
using TddBuddy.Synchronous.Process.Runner;

namespace Simple.Report.Server.Example
{
    /*
     * This is a working example of how to generate PDF reports using node and LibreOffice
     * The word file happens using docx-templater written in node to genereate docx reports based on a template
     * The pdf conversion happens using LibreOffice, both node and libreoffice 5 need to be installed on your computer to run this example.
     * 
     * Please note there is a pre-build hook to copy the NodeApp contents into the solution directory Simple.Report.Server.Example\Reporting\NodeApp from Simple.Report.Server.NodeJS
     * 
     * Please also note this example makes use of TddBuddy's Clean Architecture and Syncronus Process Execution packages. 
     * 
     * + Synchronous Process Runner - DotNetCore
     *  - Install-Package TddBuddy.Synchronous.Process.Runner.DotNetCore
     *  - Source @ https://github.com/T-rav/Synchronous-Process-Runner-DotNetCore
     *  
     * + Clean Architecture Domain - DotNetCore
     *  - Install-Package TddBuddy.CleanArchitecture.Domain.DotNetCore
     *  - Source @ https://github.com/T-rav/CleanArchitecture-DotNetCore.git
     */
    class Program
    {
        private static readonly IRenderReportUseCase RenderReportUseCase = new RenderReportUseCase(new ReportRepository("Reporting\\Templates","Reporting\\NodeApp"));

        static void Main(string[] args)
        {
            // todo : change this when running example for different output location
            var writeReportTo = "C:\\SimpleReportServer.RenderedReports";
            RenderReportWithImages(writeReportTo);
        }

        private static void RenderReportWithImages(string reportOuputDirectory)
        {
            var jsonData = File.ReadAllText("ExampleData\\WithImagesSampleData.json");

            var inputMessage = new RenderReportInputMessage
            {
                TemplateName = "ReportWithImages",
                ReportName = "ExampleReport",
                JsonModel = jsonData
            };
            
            var docxPresenter = new PropertyPresenter<IFileOutput, ErrorOutputMessage>();
            RenderReportUseCase.Execute(inputMessage, docxPresenter);

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

            Console.WriteLine($"Report output to [{pdfPresenter.SuccessContent}]");
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static PropertyPresenter<string, ErrorOutputMessage> RenderReportToPdf(string reportOuputDirectory, string reportPath)
        {
            var pdfPresenter = new PropertyPresenter<string, ErrorOutputMessage>();
            var executor = new SynchronousAction(new ConvertDocxToPdfTask(reportPath, reportOuputDirectory),
                new ProcessFactory());
            executor.Execute(pdfPresenter);
            return pdfPresenter;
        }

        private static void WriteErrorsToConsole<T>(PropertyPresenter<T, ErrorOutputMessage> presenter)
        {
            Console.WriteLine("The following errors occured: ");
            Console.WriteLine(string.Join(", ", (IEnumerable<string>) presenter.ErrorContent.Errors));
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static string PersistReport(string reportOuputDirectory, IFileOutput successContent)
        {
            EnsureDirectory(reportOuputDirectory);

            var reportPath = Path.Combine(reportOuputDirectory, $"{successContent.FileName}.docx");
            RemoveOldReport(reportPath);

            WriteReport(successContent, reportPath);

            return reportPath;
        }

        private static void WriteReport(IFileOutput successContent, string reportPath)
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

        private static void RemoveOldReport(string reportPath)
        {
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }
        }

        private static void EnsureDirectory(string reportOuputDirectory)
        {
            if (!Directory.Exists(reportOuputDirectory))
            {
                Directory.CreateDirectory(reportOuputDirectory);
            }
        }
    }
}