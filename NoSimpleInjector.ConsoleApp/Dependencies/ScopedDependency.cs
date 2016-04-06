using System;

namespace NoSimpleInjector.ConsoleApp.Dependencies
{
    public class ScopedDependency : IScopedDependency, IDisposable
    {
        public Guid Uid { get; set; } = Guid.NewGuid();

        public void Dispose()
        {
            Console.Out.WriteLine("ScopedDependency.Dispose has been triggered !!!");
        }
    }
}