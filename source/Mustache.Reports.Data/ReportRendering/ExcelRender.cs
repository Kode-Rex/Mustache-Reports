using StoneAge.Synchronous.Process.Runner.PipeLineTask;

namespace Mustache.Reports.Data.ReportRendering
{
    public class ExcelRender : NodePipeLineTask
    {
        public ExcelRender(string applicationPath, string templatePath, string templateData, int sheetNumber)
            : base(applicationPath, $"-t \"{templatePath}\" -d \"{templateData}\" -r \"excel\" -n \"{sheetNumber}\" ")
        {
        }

        public override int ProcessTimeout()
        {
            return -1;
        }
    }
}