using ExecutionLens.Debugging.DOMAIN.Enums;

namespace ExecutionLens.Debugging.APPLICATION.Contracts;

public interface IReplayService
{
    Task<ResultStatus> Replay(string logId);
}
