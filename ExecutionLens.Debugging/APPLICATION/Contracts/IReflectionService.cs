using ExecutionLens.Debugging.DOMAIN.Models;

namespace ExecutionLens.Debugging.APPLICATION.Contracts;

internal interface IReflectionService
{
    object CreateInstance(Mock mock, Mock? parent = null);
}
