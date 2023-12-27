using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Models;
using System.Diagnostics;

namespace Debugging.APPLICATION.Implementations;

internal class InterceptorService : IInterceptorService
{
    private readonly List<MethodMock> _mocks;
    public InterceptorService(List<MethodMock> mocks) => _mocks = mocks;

    [DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        foreach (var mock in _mocks)
        {
            if (mock.Method == invocation.Method.Name)
            {
                _mocks.Remove(mock);

                var mockInputs = mock.Input;
                for (int i = 0; i < mockInputs?.Length; i++)
                {
                    invocation.SetArgumentValue(i, mockInputs[i]);
                }

                invocation.ReturnValue = mock.Output;

                invocation.Proceed();

                break;
            }
        }
    }
}
