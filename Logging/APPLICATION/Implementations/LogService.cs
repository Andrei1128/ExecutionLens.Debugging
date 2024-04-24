using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Common.DOMAIN.Models;
using Common.PERSISTANCE.Contracts;
using Common.DOMAIN.Models;

namespace PostMortem.Logging.APPLICATION.Implementations;

internal class LogService(ILogRepository _logRepository) : ILogService
{
    private readonly Stack<MethodLog> CallStack = new();
    private MethodLog? Root = null;
    private MethodLog? Current = null;
    public void AddLogEntry(MethodEntry logEntry)
    {
        bool isInRoot = Current is null;

        Current = new MethodLog() 
        {
            Class = logEntry.Class,
            Method = logEntry.Method,
            Input = logEntry.Input?.Length > 0 ? logEntry.Input : null,
            EntryTime = logEntry.Time
        };

        if (Root is null)
        {
            Root = Current;
        }
        else if (isInRoot || !CallStack.TryPeek(out MethodLog? parent))
        {
            Root.Interactions ??= [];
            Root.Interactions.Add(Current);
        }
        else
        {
            parent.Interactions ??= [];
            parent.Interactions.Add(Current);
        }

        CallStack.Push(Current);
    }
    public void AddLogExit(MethodExit logExit)
    {
        CallStack.Pop();

        Current!.Output =  logExit.Output;
        Current!.HasException = logExit.HasException;
        Current!.ExitTime = logExit.Time;

        if (CallStack.TryPeek(out MethodLog? current))
            Current = current;
    }

    public void AddInformation(InformationLog log)
    {
        if (Current is not null)
        {
            Current.Informations ??= [];
            Current.Informations.Add(log);
        }
    }

    public async Task<string> Write() => await _logRepository.Add(Root!);
}
