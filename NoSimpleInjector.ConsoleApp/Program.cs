using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NoSimpleInjector.AzureWebJobs;
using NoSimpleInjector.AzureWebJobs.Queues;
using NoSimpleInjector.AzureWebJobs.ServiceBus;
using NoSimpleInjector.ConsoleApp.Dependencies;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace NoSimpleInjector.ConsoleApp
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            // Populate the queue for the test
            PopulateQueues();

            // Init IoC container
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();

            // Register dependencies
            container.RegisterSingleton<ISingletonDependency, SingletonDependency>();
            container.Register<IScopedDependency, ScopedDependency>(Lifestyle.Scoped);

            // Verify the container
            container.Verify();

            // Init webjob config
            var config = new JobHostConfiguration()
            {
                NameResolver = new ConfigNameResolver(),
                JobActivator = new SimpleInjectorJobActivator(container),
                DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString,
                StorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString
            };

            // Use a specific processor factory to handle scope.
            config.Queues.QueueProcessorFactory = new ScopedQueueProcessorFactory(container);
            
            // Set the service bus connection string
            var servicebusConfig = new ServiceBusConfiguration
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ServiceBusQueue"].ConnectionString
            };

            // Use a specific messaging provider to handle scope.
            servicebusConfig.MessagingProvider = new ScopedMessagingProvider(servicebusConfig, container);
            config.UseServiceBus(servicebusConfig);
            
            // Starts the job.
            new JobHost(config).RunAndBlock();
        }

        private static void PopulateQueues()
        {
            var queueName = ConfigurationManager.AppSettings.Get("MyQueueName");
            var queueConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            var storageAccount = CloudStorageAccount.Parse(queueConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(queueName);
            queue.AddMessage(new CloudQueueMessage("Hello, World"));
            queue.AddMessage(new CloudQueueMessage("Hello, World"));


            var sbQueueName = ConfigurationManager.AppSettings.Get("MyServiceBusQueueName");
            var sbQueueConnectionString = ConfigurationManager.ConnectionStrings["ServiceBusQueue"].ConnectionString;

            var messagingFactory = MessagingFactory.CreateFromConnectionString(sbQueueConnectionString);
            var messageSender = messagingFactory.CreateMessageSender(sbQueueName);

            // Send 2 messages
            messageSender.Send(new BrokeredMessage());
            messageSender.Send(new BrokeredMessage());
        }
    }
}