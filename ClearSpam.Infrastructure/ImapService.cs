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
                        ImapClient = new ImapClient(account.Server, account.Ssl);

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
                    From = new System.Net.Mail.MailAddress(message.From.Address, message.From.DisplayName),
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
                        mailMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                if (message.To != null)
                {
                    foreach (var to in message.To)
                    {
                        mailMessage.To.Add(new System.Net.Mail.MailAddress(to.Address.Replace(";", ""), to.DisplayName));
                    }
                }

                if (message.ReplyTo != null)
                {
                    foreach (var replyTo in message.ReplyTo)
                    {
                        mailMessage.ReplyToList.Add(new System.Net.Mail.MailAddress(replyTo.Address.Replace(";", ""), replyTo.DisplayName));
                    }
                }

                if (message.Cc != null)
                {
                    foreach (var cc in message.Cc)
                    {
                        mailMessage.CC.Add(new System.Net.Mail.MailAddress(cc.Address.Replace(";", ""), cc.DisplayName));
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
    }
}
