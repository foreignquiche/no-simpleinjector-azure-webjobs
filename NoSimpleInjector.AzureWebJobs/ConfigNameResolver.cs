using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;

namespace NoSimpleInjector.AzureWebJobs
{
    /// <summary>
    /// Resolves %name% variables in attribute values from the config file.
    /// </summary>
    public sealed class ConfigNameResolver : INameResolver
    {
        /// <summary>
        /// Resolve a %name% to a value from the confi file. Resolution is not recursive.
        /// </summary>
        /// <param name="name">The name to resolve (without the %... %)</param>
        /// <returns>
        /// The value to which the name resolves, if the name is supported; otherwise throw an <see cref="InvalidOperationException"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException"><paramref name="name"/>has not been found in the config file or its value is empty.</exception>
        public string Resolve(string name)
        {
            var resolvedName = ConfigurationManager.AppSettings.Get(name);
            if (string.IsNullOrWhiteSpace(resolvedName))
            {
                throw new InvalidOperationException("Cannot resolve " + name);
            }

            return resolvedName;
        }
    }
}
