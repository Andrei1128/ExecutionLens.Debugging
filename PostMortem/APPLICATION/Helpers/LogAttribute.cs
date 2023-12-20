using Logging.APPLICATION.Contracts;
using Logging.DOMAIN.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logging.APPLICATION.Helpers;

[AttributeUsage(AttributeTargets.Method)]
public class LogAttribute(ILogService _logService) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        LogManager.StartLogging();

        _logService.AddLogEntry(MethodEntryFactory.Create(context));

        var executedAction = await next();

        if (executedAction.Exception is not null)
        {
            _logService.AddLogExit(MethodExitFactory.Create(executedAction.Exception));
            _logService.Write();
        }
        else if (!LoggerConfiguration.IsLoggingOnlyOnExceptions)
        {
            _logService.AddLogExit(MethodExitFactory.Create(executedAction.Result));
            _logService.Write();
        }
    }
}
