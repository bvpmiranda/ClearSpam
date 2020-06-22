using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Domain.Entities
{
    public class Rule : IEntity
    {
        private Account _account;

        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;

                if (_account != null)
                {
                    AccountId = _account.Id;
                }
            }
        }
        public string Field { get; set; }
        public string Content { get; set; }
    }
}
