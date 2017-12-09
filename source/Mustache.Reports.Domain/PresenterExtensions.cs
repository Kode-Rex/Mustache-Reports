using System;
using System.Linq;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public static class PresenterExtensions
    {
        public static void Respond<T>(this IRespondWithSuccessOrError<T, ErrorOutput> presenter, Func<T> action)
        {
            try
            {
                presenter.Respond(action());
            }
            catch (Exception e)
            {
                var errorOutput = new ErrorOutput();
                errorOutput.AddErrors(
                    e.AllMessages().ToList()
                );

                presenter.Respond(errorOutput);
            }
        }
    }
}