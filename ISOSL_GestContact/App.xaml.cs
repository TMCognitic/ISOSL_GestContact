using ISOSL_GestContact.ViewModels;
using ISOSL_GestContact.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tools.Patterns.Locator;
using Tools.Patterns.Mediator;

namespace ISOSL_GestContact
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILocator Locator 
        {
            get
            {
                return (ILocator)Resources["Locator"];
            } 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            IMessenger<OpenWindowMessage<PersonneViewModel>> openWindowMessenger = Locator.GetResource<IMessenger<OpenWindowMessage<PersonneViewModel>>>();
            openWindowMessenger.Register(OnOpenWindowMessage);            
        }

        private void OnOpenWindowMessage(OpenWindowMessage<PersonneViewModel> message)
        {
            UpdateContactWindow updateContactWindow = Locator.GetResource<UpdateContactWindow>();
            updateContactWindow.DataContext = message.ViewModel;
            updateContactWindow.ShowDialog();
        }
    }
}
