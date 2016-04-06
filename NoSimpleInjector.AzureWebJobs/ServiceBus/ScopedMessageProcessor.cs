using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace NoSimpleInjector.AzureWebJobs.ServiceBus
{
    /// <summary>
    /// Wrap the message processing into a <see cref="ExecutionContextScope"/>.
    /// </summary>
    internal class ScopedMessageProcessor : MessageProcessor
    {
        private readonly Container _container;

        /// <summary>
        /// Initializes anew instance of the <see cref="ScopedMessageProcessor"/> class.
        /// </summary>
        public ScopedMessageProcessor(OnMessageOptions messageOptions, Container container)
            : base(messageOptions)
        {
            _container = container;
        }

        /// <summary>
        /// Begin a new <see cref="ExecutionContextScope"/> before processing the message.
        /// </summary>
        public override Task<bool> BeginProcessingMessageAsync(BrokeredMessage message, CancellationToken cancellationToken)
        {
            _container.BeginExecutionContextScope();
            return base.BeginProcessingMessageAsync(message, cancellationToken);
        }

        /// <summary>
        /// Dispose the current <see cref="ExecutionContextScope"/>.
        /// </summary>
        public override Task CompleteProcessingMessageAsync(BrokeredMessage message, FunctionResult result, CancellationToken cancellationToken)
        {
            _container.GetCurrentExecutionContextScope()?.Dispose();
            return base.CompleteProcessingMessageAsync(message, result, cancellationToken);
        }
    }
}
