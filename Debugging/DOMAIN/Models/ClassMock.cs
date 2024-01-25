namespace Debugging.DOMAIN.Models;

public class ClassMock
{
    public string Class { get; set; } = string.Empty;
    public List<MethodMock> Setups { get; set; } = new();
    public List<ClassMock> Interactions { get; set; } = new();
}
