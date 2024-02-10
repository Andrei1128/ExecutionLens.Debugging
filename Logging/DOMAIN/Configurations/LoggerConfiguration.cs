namespace PostMortem.Logging.DOMAIN.Configurations;

public class LoggerConfiguration
{
    public static bool IsLoggingOnlyOnExceptions { get; private set; } = false;

    internal LoggerConfiguration() { }

    public LoggerConfiguration LogOnlyOnExceptions(bool flag = true)
    {
        IsLoggingOnlyOnExceptions = flag;
        return this;
    }
}
