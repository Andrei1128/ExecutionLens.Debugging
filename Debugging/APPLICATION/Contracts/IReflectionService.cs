using PostMortem.Debugging.DOMAIN.Models;

namespace PostMortem.Debugging.APPLICATION.Contracts;

public interface IReflectionService
{
    object CreateInstance(Mock mock, Mock? parent = null);
}
