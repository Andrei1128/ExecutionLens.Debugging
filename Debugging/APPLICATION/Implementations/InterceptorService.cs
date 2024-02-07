using Castle.DynamicProxy;
using Debugging.DOMAIN.Extensions;
using Debugging.DOMAIN.Models;

namespace Debugging.APPLICATION.Implementations;

public class InterceptorService(List<Setup> setups) : IInterceptor
{

    //[DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        foreach (var setup in setups)
        {
            if (setup.Method == invocation.Method.Name)
            {
                setups.Remove(setup);

                if (setup.Input is not null)
                {
                    var normalizedParameters = invocation.Method.NormalizeParametersType(setup.Input);

                    for (int i = 0; i < normalizedParameters?.Length; i++)
                    {
                        invocation.SetArgumentValue(i, normalizedParameters[i]);
                    }
                }

                invocation.ReturnValue = setup.Output;

                invocation.Proceed();

                break;
            }
        }
    }
}
