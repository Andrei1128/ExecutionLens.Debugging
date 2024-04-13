using Castle.DynamicProxy;
using PostMortem.Logging.DOMAIN.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.Common.DOMAIN.Models;
using Nest;
using Logging.DOMAIN.Extensions;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodEntryFactory
{
    public static MethodEntry Create(IInvocation invocation) =>
        new()
        {
            Time = DateTime.Now,
            Class = invocation.TargetType.GetClassName(),
            Method = invocation.Method.Name,
            InputTypes = invocation.Method.GetParameters().GetTypesName(),
            Input = invocation.Arguments
        };

    public static MethodEntry Create(ActionExecutingContext context) =>
        new()
        {
            Time = DateTime.Now,
            Class = context.Controller.GetType().GetClassName(),
            Method = context.ActionDescriptor.GetMethodName(),
            InputTypes = [.. context.ActionDescriptor.Parameters.Select(x => x.GetType().Name)],
            Input = [.. context.ActionArguments.Values]
        };
}
