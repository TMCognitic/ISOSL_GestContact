using ISOSL_GestContact.Models.Repositories;
using ISOSL_GestContact.Models.Services;
using ISOSL_GestContact.ViewModels.Messages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox.Cryptography;
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
            ICryptoRSA crypto = new CryptoRSA(Properties.Resources.Keys);
            ConnectionInfo connectionInfo = JsonConvert.DeserializeObject<ConnectionInfo>(crypto.Decrypter(Properties.Resources.ConnectionInfo));
            string connectionString = string.Format(Configuration.GetConnectionString("ISOSL_GestContact"), connectionInfo.Login, connectionInfo.Passwd);

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
                .AddSingleton<IConnection>(sp => new Connection(SqlClientFactory.Instance, connectionString))
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

        private class ConnectionInfo
        {
            public string Login { get; set; }
            public string Passwd { get; set; }
        }
    }
}
