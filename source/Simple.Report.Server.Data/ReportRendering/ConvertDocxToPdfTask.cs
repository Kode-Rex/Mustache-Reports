using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Simple.Report.Server.Data.ReportRendering
{
    public class ConvertDocxToPdfTask : GenericPipeLineTask
    {
        public ConvertDocxToPdfTask(string libreOfficePath, string docxPath, string ouputDirectory) : base(libreOfficePath, $"--norestore --nofirststartwizard --headless --convert-to pdf \"{docxPath}\" --outdir \"{ouputDirectory}\"")
        {
        }
    }
}
