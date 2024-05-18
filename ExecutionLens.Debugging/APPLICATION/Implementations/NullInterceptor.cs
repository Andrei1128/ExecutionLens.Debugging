using Castle.DynamicProxy;
using System.Diagnostics;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class NullInterceptor : IInterceptor
{
    [DebuggerNonUserCode]
    public void Intercept(IInvocation invocation) { }
}
