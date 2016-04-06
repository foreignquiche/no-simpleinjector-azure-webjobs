using Microsoft.Azure.WebJobs.Host;
using SimpleInjector;

namespace NoSimpleInjector.AzureWebJobs
{
    /// <summary>
    /// A job activator that uses <see cref="SimpleInjector"/> to return instance of a job type.
    /// </summary>
    public sealed class SimpleInjectorJobActivator : IJobActivator
    {
        private readonly Container _container;

        /// <summary>
        /// Initialize a new instance of the <see cref="SimpleInjectorJobActivator"/> class.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to resolve dependencies.</param>
        public SimpleInjectorJobActivator(Container container)
        {
            _container = container;
        }

        /// <summary>
        /// Returns an instance of a job type.
        /// </summary>
        /// <typeparam name="T">The job type.</typeparam>
        /// <returns>
        /// A instance of the job type.
        /// </returns>
        public T CreateInstance<T>()
        {
            return (T)_container.GetInstance(typeof(T));
        }
    }
}
