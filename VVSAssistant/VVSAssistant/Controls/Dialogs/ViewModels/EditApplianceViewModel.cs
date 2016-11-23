using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.ComponentModel;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class EditApplianceViewModel : ViewModelBase
    {
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public ApplianceViewModel Appliance { get; }
        
        public EditApplianceViewModel(ApplianceViewModel appliance, Action<EditApplianceViewModel> closeHandler, Action<EditApplianceViewModel> completionHandler)
        {
            Appliance = appliance;

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
