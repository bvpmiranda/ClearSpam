using System;

namespace ClearSpam.Application.Interfaces
{
    public interface ILogger
    {
        void Debug(string message, params object[] objects);
        void Error(string message, params object[] objects);
        void Exception(string requestName, Exception exception);
        void Handling(string requestName, object request);
        void Handled(string requestName, object response, long ellapsedMilliseconds);
        void Info(string message, params object[] objects);
        void Warning(string message, params object[] objects);
    }
}