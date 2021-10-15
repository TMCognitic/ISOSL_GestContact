using ISOSL_GestContact.Models.Repositories;
using ISOSL_GestContact.Models.Services;
using ISOSL_GestContact.ViewModels.Messages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tools.Connection.Database;
using Tools.Patterns.Locator;
using Tools.Patterns.Mediator;
using Tools.Patterns.Mvvm;

namespace ISOSL_GestContact.ViewModels
{
    public class ViewModelLocator : LocatorBase
    {
        protected override void Configure(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services
#if DEBUG
                .AddLogging(c =>
                {
                    c.SetMinimumLevel(LogLevel.Trace);
                    c.AddDebug().AddConfiguration(Configuration);
                    c.AddConsole().AddConfiguration(Configuration);
                })
#endif
                .AddDispatcher()
                .AddViews()
                .AddSingleton<IConnection>(sp => new Connection(SqlClientFactory.Instance, Configuration.GetConnectionString("ISOSL_GestContact")))
                .AddSingleton<IMessenger<PersonneViewModelMessage>, Messenger<PersonneViewModelMessage>>()
                .AddSingleton<IMessenger<OpenWindowMessage<PersonneViewModel>>, Messenger<OpenWindowMessage<PersonneViewModel>>>()
                .AddSingleton<IPersonneRepository, PersonneService>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<PaysViewModel>()
                .AddTransient<PersonneViewModel>();
        }

        public MainViewModel Main 
        { 
            get { return Container.GetService<MainViewModel>(); }
        }

        public PaysViewModel Pays
        {
            get { return Container.GetService<PaysViewModel>(); }
        }
    }
}
