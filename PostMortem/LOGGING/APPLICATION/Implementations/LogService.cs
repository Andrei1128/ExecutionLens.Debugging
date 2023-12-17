using PostMortem.LOGGING.APPLICATION.Contracts;
using PostMortem.SHARED.DOMAIN.Models;

namespace PostMortem.LOGGING.APPLICATION.Implementations;

internal class LogService(IWriteService _writeService) : ILogService
{
    private readonly Stack<MethodLog> CallStack = new();
    private MethodLog? Root = null;
    private MethodLog? Current = null;
    public void AddLogEntry(MethodEntry logEntry)
    {
        bool isInRoot = Current is null;

        Current = MethodLog.Create(logEntry);

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

        Current!.SetExit(logExit);

        if (CallStack.TryPeek(out MethodLog? current))
            Current = current;
    }

    public void Write() => _writeService.Write(Root);
}
