using System;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class GenerateOfferDialogViewModel : ViewModelBase
    {
        public OfferViewModel Offer;
        public ObservableCollection<ClientViewModel> Clients;
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public string ClientName
        {
            get { return Offer.Client.ClientInformation.Name; }
            set
            {
                Offer.Client.ClientInformation.Name = value;
                OnPropertyChanged();
                OnPropertyChanged("OfferIntro");
            }
        }

        public string ClientAddress
        {
            get { return Offer.Client.ClientInformation.Address; }
            set
            {
                Offer.Client.ClientInformation.Address = value;
                OnPropertyChanged();
                OnPropertyChanged("OfferIntro");
            }
        }

        public string ClientEmail
        {
            get { return Offer.Client.ClientInformation.Email; }
            set
            {
                Offer.Client.ClientInformation.Email = value;
                OnPropertyChanged();
            }
        }

        public string ClientPhone
        {
            get { return Offer.Client.ClientInformation.PhoneNumber; }
            set
            {
                Offer.Client.ClientInformation.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string OfferIntro
        {
            get { return Offer.OfferInformation.Intro; }
            set
            {
                Offer.OfferInformation.Intro = value;
                OnPropertyChanged();
            }
        }

        public string OfferOutro
        {
            get { return Offer.OfferInformation.Outro; }
            set
            {
                Offer.OfferInformation.Outro = value;
                OnPropertyChanged();
            }
        }

        public string OfferTitle
        {
            get { return Offer.OfferInformation.Title; }
            set { Offer.OfferInformation.Title = value; OnPropertyChanged(); }
        }

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

