using System;

namespace NoSimpleInjector.ConsoleApp.Dependencies
{
    public class SingletonDependency : ISingletonDependency
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
    }
}