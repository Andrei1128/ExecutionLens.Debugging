namespace PostMortem.DOMAIN.Models;

public class MethodLog
{
    public int Id { get; private set; } = 0;
    public MethodEntry Entry { get; private set; }
    public MethodExit Exit { get; private set; }
    public List<MethodLog> Interactions { get; } = [];

    public void SetExit(MethodExit logExit) => Exit = logExit;
    public static MethodLog Create(MethodEntry logEntry)
    {
        return new MethodLog()
        {
            Id = 0,
            Entry = logEntry
        };
    }
}
