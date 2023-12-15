using PostMortem.DOMAIN.Models;

namespace PostMortem.APPLICATION.Contracts;

public interface ILogService
{
    public void AddLogEntry(MethodEntry entry);
    public void AddLogExit(MethodExit exit);
    public void Write();
}
