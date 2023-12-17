namespace PostMortem.LOGGING.DOMAIN.Utilities;
public class LoggerConfiguration
{
    public static bool IsSupressingExceptions { get; private set; } = false;
    public static bool IsLoggingOnlyOnExceptions { get; private set; } = false;

    public LoggerConfiguration SupressExceptions(bool flag = true)
    {
        IsSupressingExceptions = flag;
        return this;
    }

    public LoggerConfiguration LogOnlyOnExceptions(bool flag = true)
    {
        IsLoggingOnlyOnExceptions = flag;
        return this;
    }
}
