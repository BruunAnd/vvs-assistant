using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    //TODO: This name is fine, but we have to change the name of the class with the same name in the component view model figure
    public class ClientViewModel : ViewModelBase
    {
        private readonly Client _client;

        private ClientInformationViewModel _clientInformation;
        private ObservableCollection<OfferViewModel> _offers;

        public ClientViewModel(Client client)
        {
            _client = client;
            _clientInformation = new ClientInformationViewModel(_client.ClientInformation);

            foreach (var offer in _client.Offers)
                _offers.Add(new OfferViewModel(offer));
        }

        public int Id
        {
            get { return _client.Id; }
            set
            {
                _client.Id = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreationDate
        {
            get { return _client.CreationDate; }
            set
            {
                _client.CreationDate = value;
                OnPropertyChanged();
            }
        }

        public ClientInformationViewModel ClientInformation
        {
            get { return _clientInformation; }
            set
            {
                _clientInformation = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OfferViewModel> Offers
        {
            get { return _offers; }
            set
            {
                //HACK: Keep an eye on how we assign the offers, may need to change it
                _offers = value;
                OnPropertyChanged();
            }
        }
    }
}
