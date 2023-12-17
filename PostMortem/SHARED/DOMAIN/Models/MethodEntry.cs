using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.LOGGING.DOMAIN.Utilities;

namespace PostMortem.SHARED.DOMAIN.Models;
public class MethodEntry
{
    public DateTime Time { get; private set; } = DateTime.Now;
    public string Class { get; private set; } = string.Empty;
    public string Method { get; private set; } = string.Empty;
    public object[]? Input { get; private set; } = null;

    public static MethodEntry Create(IInvocation invocation)
    {
        return new MethodEntry()
        {
            Time = DateTime.Now,
            Class = invocation.TargetType.GetClassName(),
            Method = invocation.Method.Name,
            Input = invocation.Arguments
        };
    }

    public static MethodEntry Create(ActionExecutingContext context)
    {
        return new MethodEntry()
        {
            Time = DateTime.Now,
            Class = context.Controller.GetType().GetClassName(),
            Method = context.ActionDescriptor.GetMethodName(),
            Input = [.. context.ActionArguments.Values]
        };
    }
}