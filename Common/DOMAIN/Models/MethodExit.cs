namespace PostMortem.Common.DOMAIN.Models;

public class MethodExit
{
    public DateTime Time { get; init; } = DateTime.Now;
    public bool HasException => Output is Exception;
    public string? OutputType { get; init; } = null;
    public object? Output { get; init; } = null;
}