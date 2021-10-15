using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tools.Patterns.Locator;
using Tools.Patterns.Mvvm.Commands;

namespace Tools.Patterns.Mvvm.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected ILocator Locator { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            if(Application.Current.Resources.Contains("Locator"))
            {
                Locator = (ILocator)Application.Current.Resources["Locator"];
            }
            else
            {
                Locator = Application.Current.Resources.Values.OfType<ILocator>().SingleOrDefault();
            }

            Type typeViewModel = GetType();
            IEnumerable<PropertyInfo> propertyInfos = typeViewModel.GetProperties()
                .Where(pi => pi.PropertyType == typeof(ICommand) || 
                             pi.PropertyType.GetInterfaces().Contains(typeof(ICommand)));

            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                ICommand command = (ICommand)propertyInfo.GetMethod.Invoke(this, null);
                PropertyChanged += (s, e) => command.RaiseCanExecuteChanged();
            }
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if(!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression is null)
                throw new InvalidOperationException("It's not a member expression");

            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo is null)
                throw new InvalidOperationException("It's not a property");

            RaisePropertyChanged(propertyInfo.Name);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
