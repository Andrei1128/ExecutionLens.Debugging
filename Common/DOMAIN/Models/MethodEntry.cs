namespace PostMortem.Common.DOMAIN.Models;

public class MethodEntry
{
    public DateTime Time { get; init ; } = DateTime.Now;
    public string Class { get; init; } = string.Empty;
    public string Method { get; init; } = string.Empty;
    public object[]? Input { get; init; } = null;
}