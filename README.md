# no-simpleinjector-azure-webjobs
Non official SimpleInjector extensions for Microsoft Azure Webjob (you can download it from [nuget](https://www.nuget.org/packages/NoSimpleInjector.AzureWebJobs/)).

This libray tries to compiled Azure Webjob utilities for SimpleInjector. I've gather information from the projects I've worked on and information from StackOverflow :

- [SimpleInjector - Azure WebJob with TimerTrigger - Register IDisposable](http://stackoverflow.com/questions/35163426/simpleinjector-azure-webjob-with-timertrigger-register-idisposable)
- [Dependency injection using Azure WebJobs SDK?](http://stackoverflow.com/questions/30328775/dependency-injection-using-azure-webjobs-sdk)

For the moment, the libray only contains few classes :

- An implementation of [IJobActivator](https://github.com/Azure/azure-webjobs-sdk/blob/master/src/Microsoft.Azure.WebJobs.Host/IJobActivator.cs) that allows you to inject your dependencies into triggered job.
- An implementation of the [IQueueProcessorFactory](https://github.com/Azure/azure-webjobs-sdk/blob/master/src/Microsoft.Azure.WebJobs.Host/Queues/IQueueProcessorFactory.cs) that allows to wrap the execution of a QueueTrigger job into a scope.
- A custom [MessagingProvider](https://github.com/Azure/azure-webjobs-sdk/blob/b1fb20b0edf0530d62127d6dd9f50cc9345ca7b6/src/Microsoft.Azure.WebJobs.ServiceBus/MessagingProvider.cs) that wrap the execution of a ServicebusTrigger job into a scope.

You can find a [console app](https://github.com/foreignquiche/no-simpleinjector-azure-webjobs/tree/master/NoSimpleInjector.ConsoleApp) project that shows you how to use this classes into your webjob.

- Use a JobActivator:
```
using NoSimpleInjector.AzureWebJobs;
...
private static void Main()
{
  // Init IoC container
  var container = new Container();
  
  // Register your dependencies
  ...
  
  // Verify the container
  container.Verify();
  
  // Init webjob config
  var config = new JobHostConfiguration()
  {
    JobActivator = new SimpleInjectorJobActivator(container),
  };
  
  // Starts the job.
  new JobHost(config).RunAndBlock();
}
```

- Scope ServiceBus message processing:
```
using NoSimpleInjector.AzureWebJobs;
...
private static void Main()
{
  // Init IoC container
  var container = new Container();
  
  // Register your dependencies
  ...
  
  // Verify the container
  container.Verify();
  
  // Init webjob config
  var config = new JobHostConfiguration()
  {
    JobActivator = new SimpleInjectorJobActivator(container),
  };
  
  // Set the service bus connection string
  var servicebusConfig = new ServiceBusConfiguration
  {
    ConnectionString = ConfigurationManager.ConnectionStrings["MyServiceBusConnectionString"].ConnectionString
  };

  // Use a specific messaging provider to handle scope.
  servicebusConfig.MessagingProvider = new ScopedMessagingProvider(servicebusConfig, container);
  config.UseServiceBus(servicebusConfig);
  
  // Starts the job.
  new JobHost(config).RunAndBlock();
}
```

- Scope Azure Storage Queue message processing:
```
using NoSimpleInjector.AzureWebJobs;
...
private static void Main()
{
  // Init IoC container
  var container = new Container();
  
  // Register your dependencies
  ...
  
  // Verify the container
  container.Verify();
  
  // Init webjob config
  var config = new JobHostConfiguration()
  {
    JobActivator = new SimpleInjectorJobActivator(container),
  };

  // Use a specific queue processor factory to handle scope.
  config.Queues.QueueProcessorFactory = new ScopedQueueProcessorFactory(container);
  
  // Starts the job.
  new JobHost(config).RunAndBlock();
}
```
