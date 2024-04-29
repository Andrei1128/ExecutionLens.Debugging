namespace ExecutionLens.Debugging.APPLICATION.Contracts;

public interface IReplayService
{
    Task Replay(string logId);
}
