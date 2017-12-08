using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;
using TddBuddy.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Boundary.Rendering
{
    public interface IRenderWordReportUseCase
    {
        void Execute(RenderReportInput input, PropertyPresenter<IFileOutput, ErrorOutput> presenter);
    }
}