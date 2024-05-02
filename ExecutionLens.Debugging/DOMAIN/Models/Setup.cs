namespace ExecutionLens.Debugging.DOMAIN.Models;

internal class Setup
{
    public string Method { get; set; } = string.Empty;
    public Property[]? Input { get; set; } = null;
    public Property? Output { get; set; } = null;
}
