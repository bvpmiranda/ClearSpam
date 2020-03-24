namespace ClearSpam.Application.Interfaces
{
    public interface IClearSpamService
    {
        void Start();

        void Stop();
        
        void Restart();

        void ProcessRules();

        void ProcessRules(string userId);

        void ProcessRules(int accountId);

        void ProcessRules(int accountId, int ruleId);
    }
}
