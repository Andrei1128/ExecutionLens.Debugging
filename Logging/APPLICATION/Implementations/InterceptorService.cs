using Castle.DynamicProxy;
using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.DOMAIN.Factories;
using PostMortem.Logging.DOMAIN.Utilities;

namespace PostMortem.Logging.APPLICATION.Implementations;

internal class InterceptorService(ILogService _logService, LogManager _logManager) : IInterceptorService
{
    public void Intercept(IInvocation invocation)
    {
        if (!_logManager.IsLogging)
        {
            invocation.Proceed();
        }
        else
            try
            {
                _logService.AddLogEntry(MethodEntryFactory.Create(invocation));

                invocation.Proceed();

                _logService.AddLogExit(MethodExitFactory.Create(invocation));

            }
            catch (Exception ex)
            {
                _logService.AddLogExit(MethodExitFactory.Create(ex));

                throw;
            }
    }
}
