using Microsoft.Azure.WebJobs.ServiceBus;
using SimpleInjector;

namespace NoSimpleInjector.AzureWebJobs.ServiceBus
{
    /// <summary>
    /// A <see cref="MessagingProvider"/> that create scoped <see cref="MessageProcessor"/>.
    /// </summary>
    public sealed class ScopedMessagingProvider : MessagingProvider
    {
        private readonly ServiceBusConfiguration _config;
        private readonly Container _container;

        /// <summary>
        /// Initializes anew instance of the <see cref="ScopedMessagingProvider"/> class.
        /// </summary>
        /// <param name="config">The <see cref="ServiceBusConfiguration"/> to use.</param>
        /// <param name="container">The <see cref="Container"/> to create new scopes.</param>
        public ScopedMessagingProvider(ServiceBusConfiguration config, Container container)
            : base(config)
        {
            _config = config;
            _container = container;
        }

        /// <summary>
        /// Creates a <see cref="ScopedMessageProcessor"/> for the specified ServiceBus entity.
        /// </summary>
        public override MessageProcessor CreateMessageProcessor(string entityPath)
        {
            return new ScopedMessageProcessor(_config.MessageOptions, _container);
        }
    }
}
