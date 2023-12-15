using Microsoft.AspNetCore.Mvc.Filters;
using PostMortem.APPLICATION.Contracts;
using PostMortem.DOMAIN.Models;
using PostMortem.DOMAIN.Utilities;

namespace PostMortem.APPLICATION.Implementations;

[AttributeUsage(AttributeTargets.Method)]
public class StructuredLoggingAttribute(ILogService _logService) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        LogManager.StartLogging();

        _logService.AddLogEntry(MethodEntry.Create(context));

        var executedAction = await next();

        _logService.AddLogExit(MethodExit.Create(executedAction.Result));

        if (!LoggerConfiguration.IsLoggingOnlyOnExceptions || executedAction.Exception is not null)
            _logService.Write();
    }
}
