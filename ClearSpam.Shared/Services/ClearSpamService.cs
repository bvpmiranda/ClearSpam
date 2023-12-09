using ClearSpam.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Rule = ClearSpam.Entities.Rule;

namespace ClearSpam.Services
{
    public class ClearSpamService : IClearSpamService
    {
        private readonly ClearSpamContext _context;
        private readonly IImapService _imapService;
        private readonly ILogger<ClearSpamService> _logger;

        public ClearSpamService(ClearSpamContext context, IImapService imapService, ILogger<ClearSpamService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _imapService = imapService ?? throw new ArgumentNullException(nameof(imapService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessRulesAsync()
        {
            _logger.LogInformation("Processing rules for all accounts");

            var accounts = await _context.Accounts.Include(x => x.Rules).ToListAsync();

            foreach (var account in accounts)
            {
                ProcessAccount(account);
            }
        }

        public async Task ProcessRulesAsync(string userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null)
                return;            
            
            _logger.LogInformation($"Processing rules for user '{user.UserName}'");

            var accounts = await _context.Accounts.Where(x => x.UserId == userId).Include(x => x.Rules).ToListAsync();

            foreach (var account in accounts)
            {
                ProcessAccount(account);
            }
        }

        public async Task ProcessRulesAsync(int accountId)
        {
            var account = await _context.Accounts.Where(x => x.Id == accountId).Include(x => x.Rules).FirstOrDefaultAsync();

            if (account == null)
                return;

            ProcessAccount(account);
        }

        public async Task ProcessRuleAsync(int accountId, int ruleId)
        {
            var account = await _context.Accounts.Where(x => x.Id == accountId).FirstOrDefaultAsync();

            if (account == null)
                return;

            var rule = await _context.Rules.Where(x => x.AccountId == accountId && x.Id == ruleId).FirstOrDefaultAsync();

            if (rule == null)
                return;

            ProcessAccount(account, true);
        }

        private void ProcessAccount(Account account, bool singleRule = false)
        {
            _logger.LogInformation($"Processing rule{(singleRule ? $" '{account.Rules.Single()}'" : "s")} for account '{account.Name}'");

            try
            {
                _imapService.Account = account;
                var messages = _imapService.GetMessagesFromWatchedMailbox();
                ProcessMessages(messages);

                _logger.LogInformation($"Processing rule{(singleRule ? $" '{account.Rules.Single()}'" : "s")} for account '{account.Name}' finished successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Processing rule{(singleRule ? $" '{account.Rules.Single()}'" : "s")} for account '{account.Name}' finished with errors: {ex.Message}");
            }
        }

        private void ProcessMessages(IList<(long, MailMessage)> messages)
        {
            const int ThreadsCount = 10;

            if (messages.Count == 0)
                return;

            if (messages.Count == 1)
            {
                var message = messages.Single();

                ProcessMessage(message.Item1, message.Item2);
            }

            var threads = new List<List<Task>>();

            var threadId = 0;
            for (var i = 0; i < messages.Count; i++)
            {
                if (i < ThreadsCount)
                    threads.Add(new List<Task>());

                var message = messages[i];

                threads[threadId].Add(new Task(() => ProcessMessage(message.Item1, message.Item2)));

                threadId += 1;
                if (threadId > ThreadsCount - 1)
                    threadId = 0;
            }

            var tasks = threads.SelectMany(x => x).ToArray();
            foreach (var task in tasks)
                task.Start();

            Task.WaitAll(tasks);
        }

        private void ProcessMessage(long messageId, MailMessage message)
        {
            if (DeleteMessage(message, _imapService.Account.Rules))
            {
                _imapService.DeleteMessage(messageId);
            }
        }

        private bool DeleteMessage(MailMessage message, ICollection<Rule> rules)
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
                _logger.LogInformation($"Deleting message based on from: {from}");

                return true;
            }

            if (message.From.DisplayName.ToLower().Contains(content))
            {
                _logger.LogInformation($"Deleting message based on display name of from: {message.From.DisplayName}");

                return true;
            }

            if (message.Headers == null)
                return false;

            from = string.Join("", message.Headers.GetValues("From")).Replace("\"", "");

            if (from.ToLower().Contains(content))
            {
                _logger.LogInformation($"Deleting message based on from: {from}");

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
                    _logger.LogInformation($"Deleting message based on address of reply to list entry: {ma.Address}");

                    return true;
                }

                if (ma.DisplayName.ToLower().Contains(content))
                {
                    _logger.LogInformation($"Deleting message based on display name of reply to list entry: {ma.DisplayName}");

                    return true;
                }
            }

            return false;
        }

        private bool DeleteMessageBasedOnSubject(MailMessage message, string content)
        {
            if (message.Subject.ToLower().Contains(content))
            {
                _logger.LogInformation($"Deleting message based on subject: {message.Subject}");

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
                    _logger.LogInformation($"Deleting message based on address of to list entry: {ma.Address}");

                    return true;
                }

                if (ma.DisplayName.ToLower().Contains(content))
                {
                    _logger.LogInformation($"Deleting message based on display name of to list entry: {ma.DisplayName}");

                    return true;
                }
            }

            return false;
        }
    }
}
