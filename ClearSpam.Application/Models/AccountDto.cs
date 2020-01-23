using ClearSpam.Application.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClearSpam.Application.Models
{
    public class AccountDto : IEntityDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string OriginalPassword { get; set; }
        [DisplayName("Watched Mailbox")]
        public string WatchedMailbox { get; set; }
        public HashSet<RuleDto> Rules { get; set; } = new HashSet<RuleDto>();
    }
}
