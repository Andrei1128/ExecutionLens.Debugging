using Castle.DynamicProxy;
using Common.DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodExitFactory
{
    public static MethodExit Create(IInvocation invocation)
    {
        Property? output = null;

        if (invocation.ReturnValue is not null)
        {
            output = new Property()
            {
                Type = invocation.ReturnValue.GetType().Name,
                Value = JsonConvert.SerializeObject(invocation.ReturnValue, Formatting.Indented)
            };
        };

        return new MethodExit()
        {
            Time = DateTime.Now,
            HasException = invocation.ReturnValue is Exception,
            Output = output
        };
    }

    public static MethodExit Create(Exception ex)
    {
        var output = new Property()
        {
            Type = ex.GetType().Name,
            Value = JsonConvert.SerializeObject(ex, Formatting.Indented)
        };

        return new MethodExit()
        {
            Time = DateTime.Now,
            HasException = true,
            Output = output
        };
    }

    public static MethodExit Create(IActionResult result)
    {
        var output = new Property()
        {
            Type = result.GetType().Name,
            Value = JsonConvert.SerializeObject(result, Formatting.Indented)
        };

        return new MethodExit()
        {
            Time = DateTime.Now,
            HasException = result is Exception,
            Output = output
        };
    }
}