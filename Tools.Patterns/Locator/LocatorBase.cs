using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tools.Patterns.Locator
{
    public abstract class LocatorBase : ILocator
    {
        protected IConfiguration Configuration { get; private set; }
        protected IServiceProvider Container { get; private set; }

        protected LocatorBase()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            Configure(configurationBuilder);
            Configuration = configurationBuilder.Build();

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            Container = services.BuildServiceProvider();
        }
        protected abstract void Configure(IConfigurationBuilder configurationBuilder);
        protected abstract void ConfigureServices(IServiceCollection services);

        public TResource GetResource<TResource>()
        {
            return (TResource)GetResource(typeof(TResource));
        }

        public object GetResource(Type resourceType)
        {
            return Container.GetService(resourceType);
        }
    }
}
