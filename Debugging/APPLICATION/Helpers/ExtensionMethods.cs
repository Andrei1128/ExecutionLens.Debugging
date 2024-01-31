using Debugging.DOMAIN.Models;
using PostMortem.SHARED.DOMAIN.Models;
using System.Reflection;

namespace Debugging.APPLICATION.Helpers;

internal static class ExtensionMethods
{
    public static ClassMock ToClassMock(this MethodLog log)
    {
        ClassMock classSetup = new()
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
        {
            AddInteraction(classSetup, interaction);
        }

        return classSetup;
    }

    private static void AddInteraction(ClassMock classSetup, MethodLog interaction)
    {
        var existingClassSetup = classSetup.Interactions.FirstOrDefault(cs => cs.Class == interaction.Entry.Class);

        if (existingClassSetup == null)
        {
            existingClassSetup = new ClassMock { Class = interaction.Entry.Class };
            classSetup.Interactions.Add(existingClassSetup);
        }

        MethodMock methodSetup = new()
        {
            Method = interaction.Entry.Method,
            Input = interaction.Entry.Input,
            Output = interaction.Exit.Output
        };

        existingClassSetup.Setups.Add(methodSetup);

        foreach (var nestedInteraction in interaction.Interactions)
        {
            AddInteraction(existingClassSetup, nestedInteraction);
        }
    }
}
