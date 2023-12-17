using PostMortem.SHARED.DOMAIN.Models;

namespace PostMortem.LOGGING.APPLICATION.Contracts;
public interface IWriteService
{
    void Write(MethodLog log);
}

