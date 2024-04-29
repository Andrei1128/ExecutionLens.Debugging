using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.PERSISTANCE.Contracts;

public interface ILogRepository
{
    Task<string> Add(MethodLog log);
    Task<MethodLog> Get(string id);
}
