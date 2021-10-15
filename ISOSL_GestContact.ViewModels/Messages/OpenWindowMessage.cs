using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSL_GestContact.ViewModels.Messages
{
    public class OpenWindowMessage<TViewModel>
    {
        public TViewModel ViewModel { get; private set; }

        public OpenWindowMessage(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
