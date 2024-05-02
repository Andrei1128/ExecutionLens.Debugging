namespace ExecutionLens.Debugging.DOMAIN.Configurations;

public class DebuggerConfiguration
{
    public string ElasticUri { get; set; } = string.Empty;
    public string Index { get; set; } = "logs";
}
