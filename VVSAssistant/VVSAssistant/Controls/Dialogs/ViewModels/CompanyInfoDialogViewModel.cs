using System;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    internal class CompanyInfoDialogViewModel
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
