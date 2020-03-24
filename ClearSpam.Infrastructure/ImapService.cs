using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using ImapX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace ClearSpam.Infrastructure
{
    public class ImapService : IImapService, IDisposable
    {
        private AccountDto account;
        private readonly ICryptography cryptography;
        private readonly ILogger logger;

        public AccountDto Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;

                ImapClient = null;

                if (account != null)
                {
                    try
                    {
                        var protocols = System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12 |
                                        System.Security.Authentication.SslProtocols.Ssl2 | System.Security.Authentication.SslProtocols.Ssl3;
                        ImapClient = new ImapClient(account.Server, account.Port, protocols, account.Ssl);

                        if (ImapClient.Connect())
                        {
                            if (!ImapClient.Login(account.Login, cryptography.Decrypt(account.Password)))
                            {
                                throw new Exception($"Invalid credentials for {account.Name}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Unable to connect to server: {account.Server}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger?.Error(ex.Message);
                    }
                }
            }
        }

        public ImapClient ImapClient { get; set; }

        public ImapService(ICryptography cryptography, ILogger logger)
        {
            this.cryptography = cryptography ?? throw new ArgumentNullException(nameof(cryptography));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose()
        {
            ImapClient.Dispose();
        }

        public IEnumerable<string> GetMailboxesList()
        {
            return ImapClient.Folders.Select(x => x.Name);
        }

        public IEnumerable<(long Id, MailMessage Message)> GetMessagesFromWatchedMailbox()
        {
            var folder = ImapClient.Folders.FirstOrDefault(x => x.Name == Account.WatchedMailbox);

            var messages = folder.Search("ALL", ImapX.Enums.MessageFetchMode.Tiny);

            var result = new List<(long Id, MailMessage Message)>();
            foreach (var message in messages)
            {
                var mailMessage = new MailMessage
                {
                    From = new System.Net.Mail.MailAddress(CleanupEmailAddress(message.From.Address), message.From.DisplayName),
                    Subject = message.Subject
                };

                try
                {
                    mailMessage.Body = message.Body.HasHtml ? message.Body.Html : message.Body.HasText ? message.Body.Text : null;
                }
                catch (Exception)
                {
                }

                if (message.Headers != null)
                {
                    foreach (var header in message.Headers)
                    {
                        try
                        {
                            mailMessage.Headers.Add(header.Key, header.Value);
                        }
                        catch (Exception)
                        {
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
                        catch (Exception)
                        {
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
                        catch (Exception)
                        {
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
                        catch (Exception)
                        {
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
