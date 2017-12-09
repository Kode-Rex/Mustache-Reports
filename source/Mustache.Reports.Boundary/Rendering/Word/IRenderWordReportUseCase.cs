using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Presenters;

namespace Mustache.Reports.Boundary.Rendering.Word
{
    public interface IRenderWordReportUseCase
    {
        void Execute(RenderReportInput input, PropertyPresenter<IWordFileOutput, ErrorOutput> presenter);
    }
}