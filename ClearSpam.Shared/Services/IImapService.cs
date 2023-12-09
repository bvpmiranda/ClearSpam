using ClearSpam.Entities;
using System.Collections.Generic;
using System.Net.Mail;

namespace ClearSpam.Services
{
    public interface IImapService
    {
        Account Account { get; set; }
        IEnumerable<string> GetMailboxesList();
        IList<(long Id, MailMessage Message)> GetMessagesFromWatchedMailbox();
        void DeleteMessage(long id);
    }
}
