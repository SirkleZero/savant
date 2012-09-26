using System;
using System.Reflection;
using Microsoft.VisualStudio.Shell;

namespace VisualStudio.Interop
{
    public static class ExceptionHelper
    {
        private const string LogEntrySource = "Savant";

        public static void WriteToActivityLog(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            exception = ExceptionHelper.Unwrap(exception);

            ActivityLog.LogError(LogEntrySource, exception.Message + exception.StackTrace);
        }

        public static Exception Unwrap(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            if (exception.InnerException == null)
            {
                return exception;
            }

            if (exception is AggregateException || exception is TargetInvocationException)
            {
                return exception.GetBaseException();
            }

            return exception;
        }
    }
}
