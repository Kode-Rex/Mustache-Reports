using StoneAge.CleanArchitecture.Domain.Messages;
using StoneAge.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Example.Console.Domain
{
    public interface IConsolePresenter : IRespondWithSuccessOrError<IFileOutput, ErrorOutput>
    {
        void Render(string outputDirectory);
    }
}
