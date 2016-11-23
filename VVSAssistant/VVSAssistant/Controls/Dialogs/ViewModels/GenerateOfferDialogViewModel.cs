using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using VVSAssistant.Models;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common;
using VVSAssistant.Events;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class GenerateOfferDialogViewModel : NotifyPropertyChanged
    {
        private Offer _offer;
        public Offer Offer
        {
            get { return _offer; }
            set { _offer = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Client> Clients;
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public GenerateOfferDialogViewModel(Offer offer, ObservableCollection<Client> clients, IDialogCoordinator dialogCoordinator, Action<GenerateOfferDialogViewModel> closeHandler)
        {
            Offer = offer;
            Offer.Client.ClientInformation.PropertyChanged += OfferInformationChanged;
            Offer.OfferInformation.PropertyChanged += OfferInformationChanged;

            Clients = clients;
            SaveCommand = new RelayCommand(x =>
            {
                ConfirmSaveDialog(closeHandler);
            }, x => VerifyRequiredInformation());

            CloseCommand = new RelayCommand(x =>
            {
                CancelOfferGeneration();
                closeHandler(this);
            });
        }

        private void OfferInformationChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.NotifyCanExecuteChanged();
        }

        private void ConfirmSaveDialog(Action<GenerateOfferDialogViewModel> closeHandler)
        {
            VVSAssistantEvents.OnSaveOfferButtonPressed(Offer);
            closeHandler(this);
        }

        private void CancelOfferGeneration()
        {
            Offer.Client = new Client();
            Offer.OfferInformation = new OfferInformation();
        }

        private bool VerifyRequiredInformation()
        {
            if (!string.IsNullOrEmpty(Offer.Client.ClientInformation.Name) &&
                !string.IsNullOrEmpty(Offer.Client.ClientInformation.Email) &&
                !string.IsNullOrEmpty(Offer.Client.ClientInformation.Address) &&
                !string.IsNullOrEmpty(Offer.Client.ClientInformation.PhoneNumber) &&
                !string.IsNullOrEmpty(Offer.OfferInformation.Title) &&
                !string.IsNullOrEmpty(Offer.OfferInformation.Intro) &&
                !string.IsNullOrEmpty(Offer.OfferInformation.Outro))
                return true;
            else
                return false;
        }
    }
}

