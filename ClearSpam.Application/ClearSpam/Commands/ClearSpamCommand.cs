using MediatR;

namespace ClearSpam.Application.ClearSpam.Commands
{
    public class ClearSpamCommand : IRequest
    {
        public string UserId { get; set; }
        public int? AccountId { get; set; }
        public int? RuleId { get; set; }

        public ClearSpamCommand()
        {
        }

        public ClearSpamCommand(string userId)
        {
            UserId = userId;
        }

        public ClearSpamCommand(int accountId)
        {
            AccountId = accountId;
        }

        public ClearSpamCommand(int accountId, int ruleId)
        {
            AccountId = accountId;
            RuleId = ruleId;
        }
    }
}
