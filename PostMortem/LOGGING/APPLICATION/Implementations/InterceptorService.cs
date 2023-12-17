using Castle.DynamicProxy;
using PostMortem.LOGGING.APPLICATION.Contracts;
using PostMortem.LOGGING.DOMAIN.Utilities;
using PostMortem.SHARED.DOMAIN.Models;

namespace PostMortem.LOGGING.APPLICATION.Implementations;

internal class InterceptorService(ILogService _logService) : IInterceptorService
{
    public void Intercept(IInvocation invocation)
    {
        if (!LogManager.IsLogging)
        {
            invocation.Proceed();
        }
        else
            try
            {
                _logService.AddLogEntry(MethodEntry.Create(invocation));

                invocation.Proceed();

                _logService.AddLogExit(MethodExit.Create(invocation));

            }
            catch (Exception ex)
            {
                _logService.AddLogExit(MethodExit.Create(ex));

                if (!LoggerConfiguration.IsSupressingExceptions)
                {
                    _logService.Write();
                    throw;
                }
            }
    }
}
