using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.APPLICATION.Contracts;
public interface IWriteService
{
    void Write(MethodLog log);
}

