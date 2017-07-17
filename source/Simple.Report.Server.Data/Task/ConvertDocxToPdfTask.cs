using TddBuddy.Synchronous.Process.Runner.PipeLineTask;

namespace Simple.Report.Server.Data.Task
{
    public class ConvertDocxToPdfTask : GenericPipeLineTask
    {
        // todo : Make libreOffice path configurable
        public ConvertDocxToPdfTask(string docxPath, string ouputDirectory) : base("C:\\Program Files (x86)\\LibreOffice 5\\program\\soffice.exe", $"--norestore --nofirststartwizard --headless --convert-to pdf \"{docxPath}\" --outdir \"{ouputDirectory}\"")
        {
        }
    }
}
