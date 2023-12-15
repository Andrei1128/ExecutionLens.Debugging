namespace PostMortem.DOMAIN.Utilities;
public class LogManager
{
    public static bool IsLogging;
    public static void StartLogging() => IsLogging = true;
}
