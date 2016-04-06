using System;

namespace NoSimpleInjector.ConsoleApp.Dependencies
{
    public interface IScopedDependency
    {
        Guid Uid { get; set; }
    }
}
