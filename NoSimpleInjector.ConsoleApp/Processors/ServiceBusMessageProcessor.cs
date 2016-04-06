using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using NoSimpleInjector.ConsoleApp.Dependencies;

namespace NoSimpleInjector.ConsoleApp.Processors
{
    public class ServiceBusMessageProcessor
    {
        private readonly IScopedDependency _scopedDependency;
        private readonly ISingletonDependency _singletonDependency;

        public ServiceBusMessageProcessor(IScopedDependency scopedDependency, ISingletonDependency singletonDependency)
        {
            _scopedDependency = scopedDependency;
            _singletonDependency = singletonDependency;
        }

        public async Task ProcessAsync(
            [ServiceBusTrigger("%MyServiceBusQueueName%")] BrokeredMessage incommingMessage)
        {
            // In the example we use WriteLineAsync to make the ProcessAsync awaitable...
            await Console.Out.WriteLineAsync("ServiceBusMessageProcessor.ProcessAsync has been triggered !!!");
            await Console.Out.WriteLineAsync($"IScopedDependency: {_scopedDependency.Uid}");
            await Console.Out.WriteLineAsync($"ISingletonDependency: {_singletonDependency.Uid}");
        }
    }
}
