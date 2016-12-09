using System;
using System.ComponentModel;
using VVSAssistant.Models;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    internal class GenerateOfferDialogViewModel : NotifyPropertyChanged
    {
        private Offer _offer;
        public Offer Offer
        {
            get { return _offer; }
            set { _offer = value; OnPropertyChanged(); }
        }

        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand PrintCommand { get; }

        public GenerateOfferDialogViewModel(Offer offer, Action<GenerateOfferDialogViewModel> closeHandler, Action<GenerateOfferDialogViewModel> completionHandler, Action<GenerateOfferDialogViewModel> printHandler)
        {
            Offer = offer;
            Offer.Client = new Client() { ClientInformation = new ClientInformation()};
            Offer.OfferInformation = new OfferInformation();
            Offer.Client.ClientInformation.PropertyChanged += OfferInformationChanged;
            Offer.OfferInformation.PropertyChanged += OfferInformationChanged;
            
            PrintCommand = new RelayCommand(x =>
            {
                printHandler(this);
            }, x => VerifyRequiredInformation());

            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
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
            PrintCommand.NotifyCanExecuteChanged();
        }

        private void CancelOfferGeneration()
        {
            Offer.Client = new Client();
            Offer.OfferInformation = new OfferInformation();
        }

        private bool VerifyRequiredInformation()
        {
            return !string.IsNullOrEmpty(Offer.Client.ClientInformation.Name) &&
                   !string.IsNullOrEmpty(Offer.Client.ClientInformation.Email) &&
                   !string.IsNullOrEmpty(Offer.Client.ClientInformation.Address) &&
                   !string.IsNullOrEmpty(Offer.OfferInformation.Title) &&
                   !string.IsNullOrEmpty(Offer.OfferInformation.Intro) &&
                   !string.IsNullOrEmpty(Offer.OfferInformation.Outro) &&
                   !string.IsNullOrEmpty(Offer.OfferInformation.Signature);
        }
    }
}

