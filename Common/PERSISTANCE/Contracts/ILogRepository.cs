using PostMortem.Common.DOMAIN.Models;

namespace Common.PERSISTANCE.Contracts;

public interface ILogRepository
{
    Task<string> Add(MethodLog log);
    Task<MethodLog> Get(string id);
}
