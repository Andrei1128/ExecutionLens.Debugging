using Castle.DynamicProxy;
using PostMortem.Logging.DOMAIN.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodEntryFactory
{
    public static MethodEntry Create(IInvocation invocation) =>
        new()
        {
            Time = DateTime.Now,
            Class = invocation.TargetType.GetClassName(),
            Method = invocation.Method.Name,
            Input = invocation.Arguments
        };

    public static MethodEntry Create(ActionExecutingContext context) =>
        new()
        {
            Time = DateTime.Now,
            Class = context.Controller.GetType().GetClassName(),
            Method = context.ActionDescriptor.GetMethodName(),
            Input = [.. context.ActionArguments.Values]
        };
}
