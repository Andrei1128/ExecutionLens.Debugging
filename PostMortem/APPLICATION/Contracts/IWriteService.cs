using PostMortem.DOMAIN.Models;

namespace PostMortem.APPLICATION.Contracts;
public interface IWriteService
{
    void Write(MethodLog log);
}

