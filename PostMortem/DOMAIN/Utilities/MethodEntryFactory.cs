using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.DOMAIN.Utilities;

internal class MethodEntryFactory
{
    public static MethodEntry Create(IInvocation invocation) => new MethodEntry()
    {
        Time = DateTime.Now,
        Class = invocation.TargetType.GetClassName(),
        Method = invocation.Method.Name,
        Input = invocation.Arguments
    };

    public static MethodEntry Create(ActionExecutingContext context) => new MethodEntry()
    {
        Time = DateTime.Now,
        Class = context.Controller.GetType().GetClassName(),
        Method = context.ActionDescriptor.GetMethodName(),
        Input = [.. context.ActionArguments.Values]
    };
}
