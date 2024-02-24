using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.DOMAIN.Factories;
using PostMortem.Common.DOMAIN.Models;
using Common.PERSISTANCE.Contracts;

namespace PostMortem.Logging.APPLICATION.Implementations;

internal class LogService(ILogRepository _logRepository) : ILogService
{
    private readonly Stack<MethodLog> CallStack = new();
    private MethodLog? Root = null;
    private MethodLog? Current = null;
    public void AddLogEntry(MethodEntry logEntry)
    {
        bool isInRoot = Current is null;

        Current = MethodLogFactory.Create(logEntry);

        if (Root is null)
        {
            Root = Current;
        }
        else if (isInRoot || !CallStack.TryPeek(out MethodLog? parent))
        {
            Root.Interactions.Add(Current);
        }
        else
        {
            parent.Interactions.Add(Current);
        }

        CallStack.Push(Current);
    }
    public void AddLogExit(MethodExit logExit)
    {
        CallStack.Pop();

        Current!.Exit = logExit;

        if (CallStack.TryPeek(out MethodLog? current))
            Current = current;
    }

    public async Task<string> Write() => await _logRepository.Add(Root!);
}
