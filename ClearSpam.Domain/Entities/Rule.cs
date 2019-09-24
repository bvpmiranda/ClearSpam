using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Domain.Entities
{
    public class Rule : IEntity
    {
        private Account account;

        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;

                if (account != null)
                {
                    AccountId = account.Id;
                }
            }
        }
        public string Field { get; set; }
        public string Content { get; set; }
    }
}
