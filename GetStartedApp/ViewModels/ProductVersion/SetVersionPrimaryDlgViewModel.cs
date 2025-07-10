using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.ProductVersion
{
    public class SetVersionPrimaryDlgViewModel : ViewModelBase, IDialogAware
    {
        public SetVersionPrimaryDlgViewModel()
        {
                
        }


        public DialogCloseListener RequestClose => throw new NotImplementedException();

        public bool CanCloseDialog()
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
