using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Linq;

namespace Microsoft.Bot.Connector
{
    public sealed class ServiceProvider
    {
        private static readonly object autoRegistrationSyncLock = new object();
        private static ServiceProvider instance;
        private readonly IServiceProvider provider;

        private ServiceProvider(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Gets the currently registered instance of the <see cref="ServiceProvider"/>.
        /// </summary>
        public static ServiceProvider Instance
        {
            get
            {
                if (!IsRegistered)
                {
                    TryAutoRegisterForBackwardCompatibility();
                }

                ThrowOnNullInstance();
                return ServiceProvider.instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the service provider is registered.
        /// </summary>
        public static bool IsRegistered
        {
            get
            {
                return ServiceProvider.instance != null;
            }
        }

        /// <summary>
        /// Gets the configuration root instance.
        /// </summary>
        public IConfigurationRoot ConfigurationRoot
        {
            get
            {
                return this.GetService<IConfigurationRoot>();
            }
        }

        /// <summary>
        /// Registers the <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="provider">The service provider instance.</param>
        public static void RegisterServiceProvider(IServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (ServiceProvider.instance == null)
            {
                ServiceProvider.instance = new ServiceProvider(provider);
            }
            else
            {
                // so we don't worry about race-conditions on ServiceProvider.instance
                throw new InvalidOperationException("The service provider can only be registered once during the AppDomain lifecycle");
            }
        }

        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance.
        /// </summary>
        /// <returns>A new logger instance.</returns>
        public ILogger CreateLogger()
        {
            return this.GetService<ILoggerFactory>().CreateLogger("Microsoft.Bot.Connector");
        }

        private static void ThrowOnNullInstance()
        {
            if (ServiceProvider.instance == null)
            {
                throw new InvalidOperationException("The service provider instance was not register. Please call RegisterServiceProvider before using ServiceProvider.Instance.");
            }
        }

        private TService GetService<TService>() where TService : class
        {
            Type serviceType = typeof(TService);
            TService service = this.provider.GetService(serviceType) as TService;

            if (service == null)
            {
                throw new InvalidOperationException($"The service \"{serviceType.FullName}\" is missing on the registered service provider. This usually means that the missing service is not available in the current platform.");
            }

            return service;
        }

        /// <summary>
        /// Tries to auto register the ASP.NET implementation of Connector for backward compatibility.
        /// </summary>
        private static void TryAutoRegisterForBackwardCompatibility()
        {
            const string AspNetBotConnectorAssemblyName = "Microsoft.Bot.Connector";
            const string AspNetBotConnectorServiceProviderFullQualifiedName = "Microsoft.Bot.Connector.BotServiceProvider";

            Assembly connectorAssembly = null;

            try
            {
                var connectorAssemblyName = new AssemblyName(AspNetBotConnectorAssemblyName);
                connectorAssembly = Assembly.Load(connectorAssemblyName);
            }
            catch (Exception)
            {
                // assembly not available
                // cannot log, because we don't have service provider to get logger from
            }

            if (connectorAssembly != null)
            {
                Type botServiceProviderType = connectorAssembly.DefinedTypes
                        .FirstOrDefault(t => t.FullName.Equals(AspNetBotConnectorServiceProviderFullQualifiedName))
                        ?.AsType();

                if (botServiceProviderType == null)
                {
                    throw new InvalidOperationException($"Auto registration of service provider failed because {AspNetBotConnectorServiceProviderFullQualifiedName} is not a type in assembly {AspNetBotConnectorAssemblyName}.");
                }

                // prevent concurrent auto registration during initialization
                lock (autoRegistrationSyncLock)
                {
                    if (!IsRegistered)
                    {
                        IServiceProvider provider = (IServiceProvider)Activator.CreateInstance(botServiceProviderType);
                        ServiceProvider.RegisterServiceProvider(provider);
                    }
                }
            }
        }
    }
}