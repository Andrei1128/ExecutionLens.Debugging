using Castle.DynamicProxy;
using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Logging.DOMAIN.Configurations;
using PostMortem.Logging.DOMAIN.Factories;
using PostMortem.Logging.DOMAIN.Utilities;

namespace PostMortem.Logging.APPLICATION.Implementations;

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
                _logService.AddLogEntry(MethodEntryFactory.Create(invocation));

                invocation.Proceed();

                _logService.AddLogExit(MethodExitFactory.Create(invocation));

            }
            catch (Exception ex)
            {
                _logService.AddLogExit(MethodExitFactory.Create(ex));

                if (!LoggerConfiguration.IsSupressingExceptions)
                {
                    _logService.Write();
                    throw;
                }
            }
    }
}
