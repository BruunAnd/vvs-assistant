using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class CompanyInfoDialogViewModel
    {
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }
        public CompanyInfoDialogViewModel(Action<CompanyInfoDialogViewModel> closeHandler, Action<CompanyInfoDialogViewModel> completionHandler)
        {
            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            });

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
