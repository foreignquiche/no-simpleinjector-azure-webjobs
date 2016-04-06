using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NoSimpleInjector.ConsoleApp.Dependencies;

namespace NoSimpleInjector.ConsoleApp.Processors
{
    public class QueueMessageProcessor
    {
        private readonly IScopedDependency _scopedDependency;
        private readonly ISingletonDependency _singletonDependency;

        public QueueMessageProcessor(IScopedDependency scopedDependency, ISingletonDependency singletonDependency)
        {
            _scopedDependency = scopedDependency;
            _singletonDependency = singletonDependency;
        }

        public async Task ProcessAsync(
            [QueueTrigger("%MyQueueName%")] string message)
        {
            // In the example we use WriteLineAsync to make the ProcessAsync awaitable...
            await Console.Out.WriteLineAsync("ServiceBusMessageProcessor.ProcessAsync has been triggered !!!");
            await Console.Out.WriteLineAsync($"IScopedDependency: {_scopedDependency.Uid}");
            await Console.Out.WriteLineAsync($"ISingletonDependency: {_singletonDependency.Uid}");
        }
    }
}
