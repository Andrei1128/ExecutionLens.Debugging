using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.DOMAIN.Utilities;

internal class MethodLogFactory
{
    public static MethodLog Create(MethodEntry logEntry) => new MethodLog()
    {
        Entry = logEntry
    };
}
