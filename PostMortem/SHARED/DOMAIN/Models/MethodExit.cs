using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace PostMortem.SHARED.DOMAIN.Models;
public class MethodExit
{
    public DateTime Time { get; private set; } = DateTime.Now;
    public bool HasException => Output is Exception;
    public object? Output { get; private set; } = null;

    public static MethodExit Create(IInvocation invocation)
    {
        return new MethodExit
        {
            Time = DateTime.Now,
            Output = invocation.ReturnValue
        };
    }

    public static MethodExit Create(Exception ex)
    {
        return new MethodExit
        {
            Time = DateTime.Now,
            Output = ex
        };
    }

    public static MethodExit Create(IActionResult result)
    {
        return new MethodExit
        {
            Time = DateTime.Now,
            Output = result
        };
    }
}