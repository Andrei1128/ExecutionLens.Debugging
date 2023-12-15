using Castle.DynamicProxy;
using PostMortem.APPLICATION.Contracts;
using PostMortem.DOMAIN.Models;
using PostMortem.DOMAIN.Utilities;

namespace PostMortem.APPLICATION.Implementations;

internal class InterceptorService(ILogService _logService) : IInterceptorService
{
    public async void Intercept(IInvocation invocation)
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
