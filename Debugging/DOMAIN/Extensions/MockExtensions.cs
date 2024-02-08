using PostMortem.Debugging.DOMAIN.Models;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Debugging.DOMAIN.Extensions;

public static class MockExtensions
{
    public static Type GetClassType(this Mock payload)
        => Type.GetType(payload.Class) ?? throw new Exception($"Type '{payload.Class}' not found!");

    public static Mock ToMock(this MethodLog log)
    {
        Mock mock = new()
        {
            Class = log.Entry.Class,
            Setups = [
                new()
                {
                    Input = log.Entry.Input,
                    Method = log.Entry.Method,
                    Output = log.Exit.Output
                }]
        };

        foreach (var interaction in log.Interactions)
            AddInteraction(mock, interaction);

        return mock;
    }

    private static void AddInteraction(Mock mock, MethodLog interaction)
    {
        Mock? existingMock = mock.Interactions.FirstOrDefault(cs => cs.Class == interaction.Entry.Class);

        if (existingMock == null)
        {
            existingMock = new Mock
            {
                Class = interaction.Entry.Class
            };
            mock.Interactions.Add(existingMock);
        }

        Setup methodSetup = new()
        {
            Method = interaction.Entry.Method,
            Input = interaction.Entry.Input,
            Output = interaction.Exit.Output
        };

        existingMock.Setups.Add(methodSetup);

        foreach (MethodLog nestedInteraction in interaction.Interactions)
            AddInteraction(existingMock, nestedInteraction);
    }
}
