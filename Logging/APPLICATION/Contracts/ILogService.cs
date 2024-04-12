using Common.DOMAIN.Models;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.APPLICATION.Contracts;

internal interface ILogService
{
    void AddLogEntry(MethodEntry entry);
    void AddLogExit(MethodExit exit);
    void AddInformation(InformationLog log);
    Task<string> Write();
}
