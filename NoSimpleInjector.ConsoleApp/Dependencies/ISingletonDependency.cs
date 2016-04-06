using System;

namespace NoSimpleInjector.ConsoleApp.Dependencies
{
    public interface ISingletonDependency
    {
        Guid Uid { get; set; }
    }
}