using ClearSpam.Application.Models;
using System.Collections.Generic;
using System.Net.Mail;

namespace ClearSpam.Application.Interfaces
{
    public interface IImapService
    {
        AccountDto Account { get; set;  }
        IEnumerable<string> GetMailboxesList();
        IEnumerable<(uint Id, MailMessage Message)> GetMessagesFromWatchedMailbox();
        void DeleteMessage(uint id);
    }
}
