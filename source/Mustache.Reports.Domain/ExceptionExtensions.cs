using System;
using System.Collections.Generic;

namespace Mustache.Reports.Domain
{
    public static class ExceptionExtensions{
        public static IEnumerable<string> AllMessages(this Exception exception)
        {
            yield return exception.Message;

            if (exception.InnerException != null)
            {
                foreach (var message in exception.InnerException.AllMessages())
                {
                    yield return message;
                }
            }
        }
    }
}