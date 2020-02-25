using System;
using ClearSpam.Application.Interfaces;

namespace ClearSpam.Infrastructure
{
    public class Logger : ILogger
    {
        private readonly NLog.ILogger logger;
        private readonly IClearSpamConfigurations clearSpamConfigurations;

        public Logger(NLog.ILogger logger, IClearSpamConfigurations clearSpamConfigurations)
        {
            this.logger = logger;
            this.clearSpamConfigurations = clearSpamConfigurations;
        }

        public void Debug(string message, params object[] objects)
        {
            Console.WriteLine($"DEBUG: {message}: {objects}");
            logger.Debug(message, objects);
        }

        public void Error(string message, params object[] objects)
        {
            Console.WriteLine($"ERROR: {message}: {objects}");
            logger.Error(message, objects);
        }

        public void Exception(string requestName, Exception exception)
        {
            var message = $"Handled {requestName} threw an exception: {exception}";

            Console.WriteLine($"ERROR: {message}");
            logger.Error(message);
        }

        public void Handled(string requestName, object response, long ellapsedMilliseconds)
        {
            var message = $"Handled {requestName} in {ellapsedMilliseconds}ms: {response}";
            Console.WriteLine($"INFO: {message}");
            logger.Info(message);

            if (ellapsedMilliseconds > clearSpamConfigurations.RequestSLA)
            {
                message = $"\tRequest handled in longer than {clearSpamConfigurations.RequestSLA}ms";
                Console.WriteLine($"WARNING: {message}");
                logger.Warn(message);
            }
        }

        public void Handling(string requestName, object request)
        {
            var message = $"Handling {requestName}: {request}";
            Console.WriteLine($"INFO: {message}");
            logger.Info(message);
        }

        public void Info(string message, params object[] objects)
        {
            Console.WriteLine($"INFO: {message}: {objects}");
            logger.Info(message, objects);
        }

        public void Warning(string message, params object[] objects)
        {
            Console.WriteLine($"WARNING: {message}: {objects}");
            logger.Warn(message, objects);
        }
    }
}