namespace PostMortem.Debugging.APPLICATION.Contracts;

public interface IReplayService
{
    Task Replay(string logId);
}
