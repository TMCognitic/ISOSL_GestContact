using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Tools.Patterns.Mvvm.ViewModels;

namespace Tools.Patterns.Mvvm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDispatcher(this IServiceCollection services)
        {
            services.AddSingleton(sp => Application.Current.Dispatcher);
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            IEnumerable<AssemblyName> assemblyNames = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Union(new AssemblyName[] { Assembly.GetEntryAssembly().GetName() }).ToList();

            foreach (AssemblyName assemblyName in assemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);
                IEnumerable<Type> viewTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Window)) && t != typeof(NavigationWindow));

                foreach (Type viewType in viewTypes)
                {
                    services.AddTransient(viewType);
                }
            }

            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services, out IDictionary<string, Type> mappers)
        {
            mappers = new Dictionary<string, Type>();

            List<AssemblyName> assemblyNames = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies().ToList();
            assemblyNames.Add(Assembly.GetEntryAssembly().GetName());

            foreach (AssemblyName assemblyName in assemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);
                IEnumerable<Type> viewTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ViewModelBase)));

                foreach (Type viewModelType in viewTypes)
                {
                    ModeAttribute modeAttribute = viewModelType.GetCustomAttribute<ModeAttribute>();

                    if (modeAttribute is null)
                    {
                        services.AddSingleton(viewModelType);
                    }
                    else
                    {
                        switch(modeAttribute.Mode)
                        {
                            case Mode.Singleton:
                                services.AddSingleton(viewModelType);
                                break;
                            case Mode.Scoped:
                                services.AddScoped(viewModelType);
                                break;
                            case Mode.Transient:
                                services.AddTransient(viewModelType);
                                break;
                        }
                    }

                    if(mappers is not null)
                    {
                        mappers.Add(viewModelType.Name, viewModelType);
                    }
                }
            }

            return services;
        }
    }
}
