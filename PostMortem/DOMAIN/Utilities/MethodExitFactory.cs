using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.DOMAIN.Utilities;

internal class MethodExitFactory
{
    public static MethodExit Create(IInvocation invocation) => new MethodExit
    {
        Time = DateTime.Now,
        Output = invocation.ReturnValue
    };

    public static MethodExit Create(Exception ex) => new MethodExit
    {
        Time = DateTime.Now,
        Output = ex
    };

    public static MethodExit Create(IActionResult result) => new MethodExit
    {
        Time = DateTime.Now,
        Output = result
    };
}
