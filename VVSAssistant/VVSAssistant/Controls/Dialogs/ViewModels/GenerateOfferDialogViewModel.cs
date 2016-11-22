using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class GenerateOfferDialogViewModel : ViewModelBase
    {
        public OfferViewModel Offer;
        public ObservableCollection<ClientViewModel> Clients;
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        private bool canSave = false;

        public GenerateOfferDialogViewModel(OfferViewModel offer, ObservableCollection<ClientViewModel> clients, IDialogCoordinator dialogCoordinator, Action<GenerateOfferDialogViewModel> closeHandler)
        {
            Offer = offer;
            Clients = clients;
            SaveCommand = new RelayCommand(x =>
            {
                ConfirmSaveDialog(closeHandler);
            }, x => true);


            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }

        private void ConfirmSaveDialog(Action<GenerateOfferDialogViewModel> closeHandler)
        {
            closeHandler(this);
        }
    }
}

