namespace ClearSpam.Application.Interfaces
{
    public interface IClearSpamConfigurations
    {
        long RequestSLA { get; }
        int PeriodInSeconds { get; }
    }
}
