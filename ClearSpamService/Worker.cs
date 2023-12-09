using ClearSpam.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam
{
    public class Worker : BackgroundService
    {
        private readonly IClearSpamService _clearSpamService;
        private readonly ILogger<Worker> _logger;

        public Worker(IClearSpamService webScrapperService, ILogger<Worker> logger)
        {
            _clearSpamService = webScrapperService ?? throw new ArgumentNullException(nameof(webScrapperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _clearSpamService.ProcessRulesAsync();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}