using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Models;
using System.Diagnostics;

namespace Debugging.APPLICATION.Implementations;

internal class InterceptorService : IInterceptorService
{
    private readonly IEnumerable<MockObject> _mocks;
    public InterceptorService(IEnumerable<MockObject> mocks) => _mocks = mocks;

    [DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        //TODO: Add input from logs
        foreach (var mock in _mocks)
        {
            if (mock.Method == invocation.Method.Name)
            {
                invocation.ReturnValue = mock.Output;
            }
        }
    }
}
