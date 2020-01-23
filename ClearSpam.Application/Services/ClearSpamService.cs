using ClearSpam.Application.ClearSpam.Commands;
using ClearSpam.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Services
{
    public class ClearSpamService : IClearSpamService, IDisposable
    {
        private readonly IMediator mediator;
        private readonly int periodInSeconds;
        private readonly Timer timer;

        private bool processing;

        private bool disposed = false;

        public ClearSpamService(IMediator mediator, IClearSpamConfigurations configuration)
        {
            this.mediator = mediator;
            periodInSeconds = configuration.PeriodInSeconds;

            timer = new Timer(new TimerCallback(ClearSpam), state: null, dueTime: Timeout.Infinite, period: Timeout.Infinite);
        }

        public void Start()
        {
            if (periodInSeconds > 0)
                timer.Change(dueTime: 0, periodInSeconds * 1000);
        }

        public void Stop()
        {
            timer.Change(dueTime: Timeout.Infinite, period: Timeout.Infinite);
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void ProcessRules(string userId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(userId);
                mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void ProcessRules(int accountId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(accountId);
                mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void ProcessRules(int accountId, int ruleId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(accountId, ruleId);
                mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    timer.Dispose();

                disposed = true;
            }
        }

        private void ClearSpam(object state = null)
        {
            if (processing)
                return;

            processing = true;

            var clearSpamCommand = new ClearSpamCommand();
            mediator.Send(clearSpamCommand, CancellationToken.None);

            processing = false;
        }
    }
}
