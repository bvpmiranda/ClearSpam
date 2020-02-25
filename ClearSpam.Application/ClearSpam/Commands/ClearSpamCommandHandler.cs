using AutoMapper;
using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using ClearSpam.Common;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.ClearSpam.Commands
{
    public class ClearSpamCommandHandler : IRequestHandler<ClearSpamCommand>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly IImapService imapService;
        private readonly ILogger logger;

        public ClearSpamCommandHandler(IRepository repository, IMapper mapper, IImapService imapService, ILogger logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.imapService = imapService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(ClearSpamCommand request, CancellationToken cancellationToken)
        {
            var accounts = new List<Account>();

            if (request.AccountId.HasValue)
            {
                var account = repository.Get<Account>(request.AccountId.Value);

                if (account != null)
                    accounts.Add(account);

                ProcessAccounts(accounts, request.RuleId);

                return Unit.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                accounts.AddRange(repository.Get<Account>(x => x.UserId == request.UserId).ToList());
                ProcessAccounts(accounts);

                return Unit.Value;
            }

            accounts.AddRange(repository.Get<Account>());
            ProcessAccounts(accounts);

            return Unit.Value;
        }

        private void ProcessAccounts(IEnumerable<Account> accounts, int? ruleId = null)
        {
            foreach (var account in accounts)
            {
                try
                {
                    if (ruleId.HasValue)
                    {
                        account.Rules = repository.Get<Rule>(x => x.AccountId == account.Id && x.Id == ruleId.Value).ToList();
                    }
                    else
                    {
                        account.Rules = repository.Get<Rule>(x => x.AccountId == account.Id).ToList();
                    }

                    imapService.Account = mapper.Map<AccountDto>(account);
                    var messages = imapService.GetMessagesFromWatchedMailbox();
                    foreach (var (MessageId, Message) in messages)
                    {
                        if (DeleteMessage(Message, imapService.Account.Rules))
                        {
                            imapService.DeleteMessage(MessageId);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        private bool DeleteMessage(MailMessage message, HashSet<RuleDto> rules)
        {
            var result = false;

            foreach (var rule in rules)
            {
                var content = rule.Content.ToLower();

                switch (rule.Field)
                {
                    case Constants.Fields.From:
                        result = DeleteMessageBasedOnFrom(message, content);
                        break;

                    case Constants.Fields.ReplyTo:
                        result = DeleteMessageBasedOnReplyTo(message, content);
                        break;

                    case Constants.Fields.Subject:
                        result = DeleteMessageBasedOnSubject(message, content);
                        break;

                    case Constants.Fields.To:
                        result = DeleteMessageBasedOnTo(message, content);
                        break;
                }

                if (result)
                    break;
            }

            return result;
        }

        private bool DeleteMessageBasedOnFrom(MailMessage message, string content)
        {
            var from = message.From.Address;

            if (from.ToLower().Contains(content))
            {
                logger.Info($"Deleting message based on from: {from}");

                return true;
            }

            if (message.Headers == null)
                return false;

            from = string.Join("", message.Headers.GetValues("From")).Replace("\"", "");

            if (from.ToLower().Contains(content))
            {
                logger.Info($"Deleting message based on from: {from}");

                return true;
            }

            return false;
        }

        private bool DeleteMessageBasedOnReplyTo(MailMessage message, string content)
        {
            foreach (var ma in message.ReplyToList)
            {
                if (ma.Address.ToLower().Contains(content))
                {
                    logger.Info($"Deleting message based on address of reply to list entry: {ma.Address}");

                    return true;
                }

                if (ma.DisplayName.ToLower().Contains(content))
                {
                    logger.Info($"Deleting message based on display name of reply to list entry: {ma.DisplayName}");

                    return true;
                }
            }

            return false;
        }

        private bool DeleteMessageBasedOnSubject(MailMessage message, string content)
        {
            //var contentEncoding = Encoding.UTF8;
            //var contentBytes = contentEncoding.GetBytes(content);

            //var subjectEncoding = message.SubjectEncoding;
            //var subjectBytes = Encoding.Convert(contentEncoding, subjectEncoding, contentBytes);

            //var subject = subjectEncoding.GetString(subjectBytes);

            if (message.Subject.ToLower().Contains(content))
            {
                logger.Info($"Deleting message based on subject: {message.Subject}");

                return true;
            }

            return false;
        }

        private bool DeleteMessageBasedOnTo(MailMessage message, string content)
        {
            foreach (var ma in message.To)
            {
                if (ma.Address.ToLower().Contains(content))
                {
                    logger.Info($"Deleting message based on address of to list entry: {ma.Address}");

                    return true;
                }

                if (ma.DisplayName.ToLower().Contains(content))
                {
                    logger.Info($"Deleting message based on display name of to list entry: {ma.DisplayName}");

                    return true;
                }
            }

            return false;
        }
    }
}
