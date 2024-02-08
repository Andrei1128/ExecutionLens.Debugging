using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.APPLICATION.Contracts;

internal interface IWriteService
{
    void Write(MethodLog log);
}

