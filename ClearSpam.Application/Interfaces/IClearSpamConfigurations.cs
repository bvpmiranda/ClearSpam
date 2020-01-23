namespace ClearSpam.Application.Interfaces
{
    public interface IClearSpamConfigurations
    {
        string ConnectionString { get; }
        long RequestSLA { get; }
        int PeriodInSeconds { get; }
    }
}
