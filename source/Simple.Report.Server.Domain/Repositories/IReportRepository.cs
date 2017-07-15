using Simple.Report.Server.Domain.Messages.Input;
using Simple.Report.Server.Domain.Messages.Output;

namespace Simple.Report.Server.Domain.Repositories
{
    public interface IReportRepository
    {
        RenderedReportOutputMessage CreateReport(RenderReportInputMessage inputMessage);
    }
}
