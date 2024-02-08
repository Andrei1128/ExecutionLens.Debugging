using Castle.DynamicProxy;
using PostMortem.Debugging.DOMAIN.Extensions;
using PostMortem.Debugging.DOMAIN.Models;
using System.Diagnostics;

namespace PostMortem.Debugging.APPLICATION.Implementations;

internal class InterceptorService(List<Setup> setups) : IInterceptor
{
    [DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        Setup setup = setups.First();

        if (setup.Method == invocation.Method.Name)
        {
            setups.RemoveAt(0);

            if (setup.Input is not null)
            {
                object[] normalizedParameters = invocation.Method.NormalizeParametersType(setup.Input);

                for (int i = 0; i < normalizedParameters?.Length; i++)
                {
                    invocation.SetArgumentValue(i, normalizedParameters[i]);
                }
            }

            invocation.ReturnValue = setup.Output;
            invocation.Proceed();
        }
    }
}
