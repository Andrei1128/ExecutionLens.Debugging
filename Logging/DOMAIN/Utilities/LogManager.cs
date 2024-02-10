namespace PostMortem.Logging.DOMAIN.Utilities;

internal class LogManager
{
    public bool IsLogging;
    public void StartLogging() => IsLogging = true;
}
