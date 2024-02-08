using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.DOMAIN.Factories;

internal class MethodExitFactory
{
    public static MethodExit Create(IInvocation invocation) =>
        new()
        {
            Time = DateTime.Now,
            Output = invocation.ReturnValue
        };

    public static MethodExit Create(Exception ex) =>
        new()
        {
            Time = DateTime.Now,
            Output = ex
        };

    public static MethodExit Create(IActionResult result) =>
        new()
        {
            Time = DateTime.Now,
            Output = result
        };
}
