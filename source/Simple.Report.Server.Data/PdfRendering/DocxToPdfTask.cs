using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Simple.Report.Server.Data.PdfRendering
{
    public class DocxToPdfTask : GenericPipeLineTask
    {
        public DocxToPdfTask(string libreOfficePath, string docxPath, string ouputDirectory) : base(libreOfficePath, $"--norestore --nofirststartwizard --headless --convert-to pdf \"{docxPath}\" --outdir \"{ouputDirectory}\"")
        {
        }
    }
}
