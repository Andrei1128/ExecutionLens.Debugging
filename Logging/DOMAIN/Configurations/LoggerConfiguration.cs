using Microsoft.Extensions.Logging;

namespace PostMortem.Logging.DOMAIN.Configurations;

public class LoggerConfiguration
{
    public bool LogOnlyOnException { get; set; } = false;
    public bool LogToConsole { get; set; } = false;
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;
}
