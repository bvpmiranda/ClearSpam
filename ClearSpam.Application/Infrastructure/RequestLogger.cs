using System;
using System.Diagnostics;
using MediatR;
using ClearSpam.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Infrastructure
{
    public class RequestLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public RequestLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;

            logger.Handling(requestName, request);

            var timer = new Stopwatch();
            timer.Start();

            TResponse response;
            try
            {
                response = await next();
                timer.Stop();

                logger.Handled(requestName, response, timer.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                logger.Exception(requestName, e);

                throw;
            }

            return response;
        }
    }
}
