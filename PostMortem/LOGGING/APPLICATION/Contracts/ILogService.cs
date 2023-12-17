using PostMortem.SHARED.DOMAIN.Models;

namespace PostMortem.LOGGING.APPLICATION.Contracts;

public interface ILogService
{
    public void AddLogEntry(MethodEntry entry);
    public void AddLogExit(MethodExit exit);
    public void Write();
}
