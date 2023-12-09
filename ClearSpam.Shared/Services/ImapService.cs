using ClearSpam.Entities;
using ImapX;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace ClearSpam.Services
{
    public class ImapService : IImapService, IDisposable
    {
        private Account _account;
        private readonly ICryptography _cryptography;
        private readonly ILogger<ImapService> _logger;

        public Account Account
        {
            get
            {
                return _account;
            }

            set
            {
                _account = value;

                ImapClient = null;

                if (_account != null)
                {
                    try
                    {
                        var protocols = System.Security.Authentication.SslProtocols.Default |
                                        System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12 |
                                        System.Security.Authentication.SslProtocols.Ssl2 | System.Security.Authentication.SslProtocols.Ssl3;
                        ImapClient = new ImapClient(_account.Server, _account.Port, _account.Ssl)
                        {
                            SslProtocol = protocols
                        };

                        if (ImapClient.Connect())
                        {
                            if (!ImapClient.Login(_account.Login, _cryptography.Decrypt(_account.Password)))
                            {
                                throw new Exception($"Invalid credentials for {_account.Name}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Unable to connect to server: {_account.Server}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, ex.Message);
                    }
                }
            }
        }

        public ImapClient ImapClient { get; set; }

        public ImapService(ICryptography cryptography, ILogger<ImapService> logger)
        {
            _cryptography = cryptography ?? throw new ArgumentNullException(nameof(cryptography));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            ImapClient?.Dispose();
        }

        public IEnumerable<string> GetMailboxesList()
        {
            return ImapClient.Folders.Select(x => x.Name);
        }

        public IList<(long Id, MailMessage Message)> GetMessagesFromWatchedMailbox()
        {
            var folder = ImapClient.Folders.FirstOrDefault(x => x.Name == Account.WatchedMailbox);

            var messages = folder?.Search("ALL", ImapX.Enums.MessageFetchMode.Tiny) ?? Array.Empty<Message>();

            var result = new List<(long Id, MailMessage Message)>();
            foreach (var message in messages)
            {
                var mailMessage = new MailMessage();

                try
                {
                    mailMessage.From = new System.Net.Mail.MailAddress(CleanupEmailAddress(message.From.Address), message.From.DisplayName);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"There was a problem parsing the From: {message.From.Address} ({message.From.DisplayName})");
                }

                try
                {
                    mailMessage.Subject = message.Subject.Trim();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"There was a problem parsing the Subject: {message.Subject}");
                }

                try
                {
                    mailMessage.Body = message.Body.HasHtml ? message.Body.Html : message.Body.HasText ? message.Body.Text : null;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"There was a problem parsing the Body: {(message.Body.HasHtml ? message.Body.Html : message.Body.HasText ? message.Body.Text : null)}");
                }

                if (message.Headers != null)
                {
                    foreach (var header in message.Headers)
                    {
                        try
                        {
                            mailMessage.Headers.Add(header.Key, header.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"There was a problem parsing the Header: {header.Key} -> {header.Value}");
                        }
                    }
                }

                if (message.To != null)
                {
                    foreach (var to in message.To)
                    {
                        try
                        {
                            mailMessage.To.Add(new System.Net.Mail.MailAddress(CleanupEmailAddress(to.Address), to.DisplayName));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"There was a problem parsing the To: {to.Address} ({to.DisplayName})");
                        }
                    }
                }

                if (message.ReplyTo != null)
                {
                    foreach (var replyTo in message.ReplyTo)
                    {
                        try
                        {
                            mailMessage.ReplyToList.Add(new System.Net.Mail.MailAddress(CleanupEmailAddress(replyTo.Address), replyTo.DisplayName));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"There was a problem parsing the ReplyTo: {replyTo.Address} ({replyTo.DisplayName})");
                        }
                    }
                }

                if (message.Cc != null)
                {
                    foreach (var cc in message.Cc)
                    {
                        try
                        {
                            mailMessage.CC.Add(new System.Net.Mail.MailAddress(CleanupEmailAddress(cc.Address), cc.DisplayName));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"There was a problem parsing the Cc: {cc.Address} ({cc.DisplayName})");
                        }
                    }
                }

                result.Add((message.UId, mailMessage));
            }

            return result;
        }

        public void DeleteMessage(long id)
        {
            var folder = ImapClient.Folders.FirstOrDefault(x => x.Name == Account.WatchedMailbox);

            var message = folder.Search(new[] { id }, ImapX.Enums.MessageFetchMode.Tiny).FirstOrDefault();

            if (message != null)
            {
                message.Seen = true;
                message.Remove();
            }
        }

        private string CleanupEmailAddress(string emailaddress)
        {
            return emailaddress.Replace(":", "").Replace(";", "").Trim();
        }
    }
}
