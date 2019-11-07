using AutoMapper;
using ClearSpam.Application.ClearSpam.Commands;
using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using ClearSpam.Common;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace ClearSpam.Application.Services
{
    public class ClearSpamService : IClearSpamService, IDisposable
    {
        private readonly IMediator mediator;
        private readonly int periodInSeconds;
        private readonly Timer timer;

        private bool processing;

        public ClearSpamService(IMediator mediator, IClearSpamConfigurations configuration)
        {
            this.mediator = mediator;
            periodInSeconds = configuration.PeriodInSeconds;

            timer = new Timer(new TimerCallback(ClearSpam), state: null, dueTime: Timeout.Infinite, period: Timeout.Infinite);
        }

        public void Start()
        {
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

        public void Dispose()
        {
            timer.Dispose();
        }

        private void ClearSpam(object state)
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
