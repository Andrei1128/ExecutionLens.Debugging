﻿using Castle.DynamicProxy;
using ExecutionLens.Debugging.DOMAIN.Extensions;
using ExecutionLens.Debugging.DOMAIN.Models;
using System.Diagnostics;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class InterceptorService(List<Setup> setups) : IInterceptor
{
    [DebuggerStepThrough]
    public void Intercept(IInvocation invocation)
    {
        Setup? setup = setups.FirstOrDefault();

        if (setup?.Method == invocation.Method.Name)
        {
            setups.RemoveAt(0);

            if (setup.Input is not null)
            {
                object[] normalizedParameters = invocation.Method.NormalizeParametersType(setup.Input);

                for (int i = 0; i < normalizedParameters.Length; i++)
                {
                    invocation.SetArgumentValue(i, normalizedParameters[i]);
                }
            }

            invocation.ReturnValue = setup.Output;
            invocation.Proceed();
        }
        else
            throw new Exception($"An exception was thrown during the original execution and has not been triggered now, or `{invocation.Method.Name}` has no setups registered!");
    }
}