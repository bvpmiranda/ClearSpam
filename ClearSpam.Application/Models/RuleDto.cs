using ClearSpam.Application.Interfaces;

namespace ClearSpam.Application.Models
{
    public class RuleDto : IEntityDto
    {
        private AccountDto account;

        public int Id { get; set; }
        public int AccountId { get; set; }
        public AccountDto Account
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
