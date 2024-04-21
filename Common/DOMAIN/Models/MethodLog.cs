using Common.DOMAIN.Models;
using Nest;
using Newtonsoft.Json;

namespace PostMortem.Common.DOMAIN.Models;

public class MethodLog
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public DateTime EntryTime { get; set; } = DateTime.Now;
    public DateTime ExitTime { get; set; } = DateTime.Now;

    public bool HasException => Output is Exception;
    public string[]? InputTypes { get; set; } = null;
    public object[]? Input { get; set; } = null;
    public string? OutputType { get; set; } = null;
    public object? Output { get; set; } = null;

    public List<InformationLog>? Informations { get; set; } = null;
    [Ignore,JsonIgnore]
    public List<MethodLog>? Interactions { get; set; } = null;
}