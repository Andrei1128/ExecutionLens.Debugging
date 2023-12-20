namespace PostMortem.SHARED.DOMAIN.Models;

public class MethodLog
{
    public int Id { get; set; }
    public MethodEntry Entry { get; set; } = default!;
    public MethodExit Exit { get; set; } = default!;
    public List<MethodLog> Interactions { get; } = [];
}
