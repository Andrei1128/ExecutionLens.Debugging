using System.Diagnostics;

namespace Debugging.DOMAIN.Models;

internal class MockObject
{
    public string Method { get; }
    public object? Output { get; }

    [DebuggerStepThrough]
    public MockObject(string method, object? output)
    {
        Method = method;
        Output = output;
    }
}
