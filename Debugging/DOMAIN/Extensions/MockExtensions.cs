using PostMortem.Debugging.DOMAIN.Models;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Debugging.DOMAIN.Extensions;

internal static class MockExtensions
{
    public static Type GetClassType(this Mock payload)
        => Type.GetType(payload.Class) ?? throw new Exception($"Type '{payload.Class}' not found!");

    public static Mock ToMock(this MethodLog log)
    {
        Mock mock = new()
        {
            Class = log.Class,
            Setups = [
                new()
                {
                    Input = log.Input,
                    Method = log.Method,
                    Output = log.Output
                }]
        };

        if (log.Interactions is not null)
        {
            foreach (var interaction in log.Interactions)
            {
                AddInteraction(mock, interaction);
            }
        }

        return mock;
    }

    private static void AddInteraction(Mock mock, MethodLog interaction)
    {
        Mock? existingMock = mock.Interactions.FirstOrDefault(cs => cs.Class == interaction.Class);

        if (existingMock == null)
        {
            existingMock = new Mock
            {
                Class = interaction.Class
            };
            mock.Interactions.Add(existingMock);
        }

        Setup methodSetup = new()
        {
            Method = interaction.Method,
            Input = interaction.Input,
            Output = interaction.Output
        };

        existingMock.Setups.Add(methodSetup);

        foreach (MethodLog nestedInteraction in interaction.Interactions)
            AddInteraction(existingMock, nestedInteraction);
    }
}
