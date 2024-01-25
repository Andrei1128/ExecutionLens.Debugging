using Castle.DynamicProxy;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Models;

namespace Debugging.APPLICATION.Implementations;

public class InterceptorService(List<MethodMock> setups) : IInterceptorService
{

    //[DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        foreach (var setup in setups)
        {
            if (setup.Method == invocation.Method.Name)
            {
                setups.Remove(setup);

                var setupInputs = setup.Input;
                for (int i = 0; i < setupInputs?.Length; i++)
                {
                    invocation.SetArgumentValue(i, setupInputs[i]);
                }

                invocation.ReturnValue = setup.Output;

                invocation.Proceed();

                break;
            }
        }
    }
}
