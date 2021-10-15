using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSL_GestContact.ViewModels
{
    public class PaysViewModel
    {
        private ObservableCollection<string> _items;

        public ObservableCollection<string> Items
        {
            get
            {
                return _items ??= new ObservableCollection<string>() { "Belgique", "France", "Italie" };
            }
        }
    }
}
