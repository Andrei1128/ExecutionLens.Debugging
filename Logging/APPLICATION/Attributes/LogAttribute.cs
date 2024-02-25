using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.DOMAIN.Factories;
using PostMortem.Logging.DOMAIN.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PostMortem.Logging.DOMAIN.Configurations;

namespace PostMortem.Logging;

[AttributeUsage(AttributeTargets.Method)]
internal class LoggerAttribute(ILogService _logService, LogManager _logManager) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logManager.StartLogging();

        _logService.AddLogEntry(MethodEntryFactory.Create(context));

        ActionExecutedContext executedAction = await next();

        if (executedAction.Exception is not null)
        {
            _logService.AddLogExit(MethodExitFactory.Create(executedAction.Exception));

            var logId = await _logService.Write();
            context.HttpContext.Items.TryAdd(nameof(logId), logId);
        }
        else if (!LoggerConfiguration.IsLoggingOnlyOnExceptions)
        {
            _logService.AddLogExit(MethodExitFactory.Create(executedAction.Result));

            var logId = await _logService.Write();
            context.HttpContext.Items.TryAdd(nameof(logId), logId);
        }
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class LogAttribute : TypeFilterAttribute
{
    public LogAttribute() : base(typeof(LoggerAttribute)) { }
}
