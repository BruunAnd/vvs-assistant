using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    //TODO: This name is fine, but we have to change the name of the class with the same name in the component view model figure
    class ClientViewModel : ViewModelBase
    {
        private readonly Client _client;
        
        private ClientInformationViewModel _clientInformation;
        private ObservableCollection<OfferViewModel> _offers;
        public bool hasInformation = false;

        public ClientViewModel(Client client)
        {
            _client = client;
            _clientInformation = new ClientInformationViewModel(client.ClientInformation);
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
                //HACK: Keep an eye on how we assign the list of offers, may need to change it
                _offers = value;
                OnPropertyChanged();
            }
        }

        public void VerifyRequiredInformation()
        {
            VerifyProperties(_client);
            VerifyProperties(ClientInformation);
        }

        private void VerifyProperties(object obj)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                if (string.IsNullOrEmpty(pi.GetValue(obj).ToString()))
                {
                    hasInformation = false;
                    return;
                }
            }
            hasInformation = true;
        }

    }
}
