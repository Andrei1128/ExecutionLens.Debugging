namespace Debugging.DOMAIN.Models;

internal class MethodMock
{
    public string Method { get; set; } = string.Empty;
    public object[]? Input { get; set; } = null;
    public object? Output { get; set; } = null;
}
