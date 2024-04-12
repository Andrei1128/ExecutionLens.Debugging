using Common.DOMAIN.Models;

namespace PostMortem.Common.DOMAIN.Models;

public class MethodLog
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public bool HasException => Output is Exception;

    public DateTime EntryTime { get; set; } = DateTime.Now;
    public DateTime ExitTime { get; set; } = DateTime.Now;

    public string? InputType { get; set; } = string.Empty;
    public object[]? Input { get; set; } = null;
    public string? OutputType { get; set; } = string.Empty;
    public object? Output { get; set; } = null;

    public List<InformationLog> Informations { get; set; } = [];
    public List<MethodLog> Interactions { get; set; } = [];
}