using Mustache.Reports.Boundary.Rendering;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Domain.Rendering
{
    public class RenderWordReportUseCase : IRenderWordReportUseCase
    {
        public void Execute(RenderReportInput input, PropertyPresenter<IFileOutput, ErrorOutput> presenter)
        {
            presenter.Respond(new WordFileOutput("cake.docx", new byte[0]));
        }
    }
}