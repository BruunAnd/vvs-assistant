using System;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.ComponentModel;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class EditApplianceViewModel : ViewModelBase
    {
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public Appliance Appliance { get; }
        
        public EditApplianceViewModel(Appliance appliance, Action<EditApplianceViewModel> closeHandler, Action<EditApplianceViewModel> completionHandler)
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
