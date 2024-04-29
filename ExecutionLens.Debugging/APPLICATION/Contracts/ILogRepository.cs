using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.APPLICATION.Contracts;

public interface ILogRepository
{
    Task<MethodLog> Get(string id);
}
