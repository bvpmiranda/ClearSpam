using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using S22.Imap;
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
                        ImapClient = new ImapClient(account.Server, account.Port, account.Login, cryptography.Decrypt(account.Password), AuthMethod.Login, account.Ssl);
                        ImapClient.DefaultMailbox = account.WatchedMailbox;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public IImapClient ImapClient { get; set; }

        public ImapService(ICryptography cryptography)
        {
            this.cryptography = cryptography ?? throw new ArgumentNullException(nameof(cryptography));
        }

        public void Dispose()
        {
            ImapClient.Dispose();
        }

        public IEnumerable<string> GetMailboxesList()
        {
            return ImapClient.ListMailboxes();
        }

        public IEnumerable<(uint Id, MailMessage Message)> GetMessagesFromWatchedMailbox()
        {
            var messageIds = ImapClient.Search(SearchCondition.All()).ToList();

            var result = new List<(uint Id, MailMessage Message)>();
            foreach (var messageId in messageIds)
            {
                result.Add((messageId, ImapClient.GetMessage(messageId, FetchOptions.HeadersOnly, seen: false)));
            }

            return result;
        }

        public void DeleteMessage(uint id)
        {
            //ImapClient.MoveMessage(id, Account.TrashMailbox);
            ImapClient.DeleteMessage(id);
            ImapClient.Expunge(ImapClient.DefaultMailbox);
        }
    }
}
