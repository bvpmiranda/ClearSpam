using System.Threading.Tasks;

namespace ClearSpam.Services
{
    public interface IClearSpamService
    {
        Task ProcessRulesAsync();
        Task ProcessRulesAsync(string userId);
        Task ProcessRulesAsync(int accountId);
        Task ProcessRuleAsync(int accountId, int ruleId);
    }
}