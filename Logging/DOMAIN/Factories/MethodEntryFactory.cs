using Castle.DynamicProxy;
using PostMortem.Logging.DOMAIN.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.Common.DOMAIN.Models;
using Common.DOMAIN.Models;
using Newtonsoft.Json;
using Nest;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodEntryFactory
{
    public static MethodEntry Create(IInvocation invocation)
    {
        int length = invocation.Arguments.Length;

        var input = new Property[length];

        for (int i = 0; i < length; i++)
        {
            input[i] = new Property()
            {
                Type = invocation.Arguments[i].GetType().Name,
                Value = JsonConvert.SerializeObject(invocation.Arguments[i], Formatting.Indented)
            };
        }

        return new MethodEntry()
        {
            Time = DateTime.Now,
            Class = invocation.TargetType.GetClassName(),
            Method = invocation.Method.Name,
            Input = [.. input]
        };
    }

    public static MethodEntry Create(ActionExecutingContext context)
    {
        object[] values = [.. context.ActionArguments.Values];
        string[] types = [.. context.ActionDescriptor.Parameters.Select(x => x.GetType().Name)];

        int length = values.Length;

        var input = new Property[length];

        for (int i = 0; i < length; i++)
        {
            input[i] = new Property()
            {
                Type = types[i],
                Value = JsonConvert.SerializeObject(values[i], Formatting.Indented)
            };
        }

        return new MethodEntry()
        {
            Time = DateTime.Now,
            Class = context.Controller.GetType().GetClassName(),
            Method = context.ActionDescriptor.GetMethodName(),
        };
    }
}
