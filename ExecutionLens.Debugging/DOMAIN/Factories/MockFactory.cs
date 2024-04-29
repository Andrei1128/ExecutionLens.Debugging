using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.DOMAIN.Factories;

internal class MockFactory
{
    public static Setup Create(string method, object[]? input, object? output)
        => new()
        {
            Method = method,
            Input = input,
            Output = output
        };
}
