using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.DOMAIN.Configurations;
using PostMortem.Logging.DOMAIN.Factories;
using PostMortem.Logging.DOMAIN.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PostMortem.Logging;

[AttributeUsage(AttributeTargets.Method)]
internal class LoggerAttribute(ILogService _logService) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        LogManager.StartLogging();

        _logService.AddLogEntry(MethodEntryFactory.Create(context));

        ActionExecutedContext executedAction = await next();

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

[AttributeUsage(AttributeTargets.Method)]
public class LogAttribute : TypeFilterAttribute
{
    public LogAttribute() : base(typeof(LoggerAttribute)) { }
}
