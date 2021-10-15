using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSL_GestContact.ViewModels.Messages
{
    public class PersonneViewModelMessage
    {
        public PersonneViewModelMessage(PersonneViewModel viewModelInstance, Actions action)
        {
            ViewModelInstance = viewModelInstance;
            Action = action;
        }

        public PersonneViewModel ViewModelInstance { get; private set; }
        public Actions Action { get; private set; }

    }
}
