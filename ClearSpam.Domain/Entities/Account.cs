using ClearSpam.Domain.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClearSpam.Domain.Entities
{
    public class Account : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string WatchedMailbox { get; set; }
        public string TrashMailbox { get; set; }
        public ICollection<Rule> Rules { get; set; } = new Collection<Rule>();
    }
}
