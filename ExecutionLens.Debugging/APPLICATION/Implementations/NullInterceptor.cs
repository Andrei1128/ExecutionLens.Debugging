using Castle.DynamicProxy;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class NullInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation) { }
}
