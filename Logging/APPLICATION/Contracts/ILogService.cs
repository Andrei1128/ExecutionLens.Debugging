using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.APPLICATION.Contracts;

internal interface ILogService
{
    public void AddLogEntry(MethodEntry entry);
    public void AddLogExit(MethodExit exit);
    public void Write();
}
