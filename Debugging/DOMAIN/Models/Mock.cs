namespace PostMortem.Debugging.DOMAIN.Models;

public class Mock
{
    public string Class { get; set; } = string.Empty;
    public List<Setup> Setups { get; set; } = [];
    public List<Mock> Interactions { get; set; } = [];
}
