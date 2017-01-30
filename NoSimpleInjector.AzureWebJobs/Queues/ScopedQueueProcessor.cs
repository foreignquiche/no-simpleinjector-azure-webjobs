using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Queues;
using Microsoft.WindowsAzure.Storage.Queue;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace NoSimpleInjector.AzureWebJobs.Queues
{
    /// <summary>
    /// Wrap the queue message processing into a <see cref="ExecutionContextScope"/>.
    /// </summary>
    internal class ScopedQueueProcessor : QueueProcessor
    {
        private readonly Container _container;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScopedQueueProcessor"/> class.
        /// </summary>
        public ScopedQueueProcessor(QueueProcessorFactoryContext context, Container container)
            : base(context)
        {
            _container = container;
        }

        /// <summary>
        /// Begin a new <see cref="ExecutionContextScope"/> before processing the message.
        /// </summary>
        public override Task<bool> BeginProcessingMessageAsync(CloudQueueMessage message, CancellationToken cancellationToken)
        {
            _container.BeginExecutionContextScope();
            return base.BeginProcessingMessageAsync(message, cancellationToken);
        }

        /// <summary>
        /// Dispose the current <see cref="ExecutionContextScope"/>.
        /// </summary>
        public override Task CompleteProcessingMessageAsync(CloudQueueMessage message, FunctionResult result,
            CancellationToken cancellationToken)
        {
            Lifestyle.Scoped.GetCurrentScope(_container)?.Dispose();
            return base.CompleteProcessingMessageAsync(message, result, cancellationToken);
        }
    }
}