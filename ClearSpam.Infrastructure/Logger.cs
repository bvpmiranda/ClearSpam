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

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug(string message, params object[] objects)
        {
            logger.Debug(message, objects);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, params object[] objects)
        {
            logger.Error(message, objects);
        }

        public void Exception(string requestName, Exception exception)
        {
            logger.Error("Handled {@requestName} threw an exception: {@exception}",
                         requestName,
                         exception.Message);
        }

        public void Handled(string requestName, object response, long ellapsedMilliseconds)
        {
            logger.Info("Handled {@requestName} in {@timeInMilliseconds}ms: {@response}",
                        requestName,
                        ellapsedMilliseconds,
                        response);

            if (ellapsedMilliseconds > clearSpamConfigurations.RequestSLA)
            {
                logger.Warn($"\tRequest handled in longer than {clearSpamConfigurations.RequestSLA}ms");
            }
        }

        public void Handling(string requestName, object request)
        {
            logger.Info("Handling {@requestName}: {@request}", requestName, request);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(string message, params object[] objects)
        {
            logger.Info(message, objects);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Warning(string message, params object[] objects)
        {
            logger.Warn(message, objects);
        }
    }
}