using Microsoft.Azure.WebJobs.Host.Queues;
using SimpleInjector;

namespace NoSimpleInjector.AzureWebJobs.Queues
{
    /// <summary>
    /// A <see cref="IQueueProcessorFactory"/> that create scoped <see cref="QueueProcessor"/>.
    /// </summary>
    public sealed class ScopedQueueProcessorFactory : IQueueProcessorFactory
    {
        private readonly Container _container;

        /// <summary>
        /// Initialize a new instance of the <see cref="ScopedQueueProcessorFactory"/> class.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to create new scopes.</param>
        public ScopedQueueProcessorFactory(Container container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates a <see cref="QueueProcessor"/> using the specified context.
        /// </summary>
        /// <param name="context">The <see cref="QueueProcessorFactoryContext"/> to use.</param>
        /// <returns>
        /// A <see cref="QueueProcessor"/> instance.
        /// </returns>
        public QueueProcessor Create(QueueProcessorFactoryContext context)
        {
            return new ScopedQueueProcessor(context, _container);
        }
    }
}
