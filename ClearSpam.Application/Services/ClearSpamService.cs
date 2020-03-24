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
        private readonly IMediator _mediator;
        private readonly int _periodInSeconds;
        private readonly Timer _timer;

        private bool _processing;

        private bool _disposed = false;

        public ClearSpamService(IMediator mediator, IClearSpamConfigurations configuration)
        {
            _mediator = mediator;
            _periodInSeconds = configuration.PeriodInSeconds;

            _timer = new Timer(new TimerCallback(ClearSpam), state: null, dueTime: Timeout.Infinite, period: Timeout.Infinite);
        }

        public void Start()
        {
            ProcessRules();

            if (_periodInSeconds > 0)
                _timer.Change(dueTime: 0, _periodInSeconds * 1000);
        }

        public void Stop()
        {
            _timer.Change(dueTime: Timeout.Infinite, period: Timeout.Infinite);
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void ProcessRules()
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand();
                _mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void ProcessRules(string userId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(userId);
                _mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void ProcessRules(int accountId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(accountId);
                _mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void ProcessRules(int accountId, int ruleId)
        {
            Task.Run(() => {
                var clearSpamCommand = new ClearSpamCommand(accountId, ruleId);
                _mediator.Send(clearSpamCommand, CancellationToken.None);
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                    _timer.Dispose();

                _disposed = true;
            }
        }

        private void ClearSpam(object state = null)
        {
            if (_processing)
                return;

            _processing = true;

            var clearSpamCommand = new ClearSpamCommand();
            _mediator.Send(clearSpamCommand, CancellationToken.None);

            _processing = false;
        }
    }
}
