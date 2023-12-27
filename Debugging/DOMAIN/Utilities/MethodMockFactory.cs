using Debugging.DOMAIN.Models;

namespace Debugging.DOMAIN.Utilities;

internal class MethodMockFactory
{
    public static MethodMock Create(string method, object[]? input, object? output) => new MethodMock
    {
        Method = method,
        Input = input,
        Output = output
    };
}
