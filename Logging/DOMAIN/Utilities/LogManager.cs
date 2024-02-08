namespace PostMortem.Logging.DOMAIN.Utilities;

internal class LogManager
{
    public static bool IsLogging;
    public static void StartLogging() => IsLogging = true;
}
