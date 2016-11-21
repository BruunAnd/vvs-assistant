using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class ClientInformationViewModel : ViewModelBase
    {
        private ClientInformation _clientInformation;

        public ClientInformationViewModel(ClientInformation clientInformation)
        {
            _clientInformation = clientInformation;
        }

        public int Id
        {
            get { return _clientInformation.Id; }
            set
            {
                _clientInformation.Id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _clientInformation.Name; }
            set
            {
                _clientInformation.Name = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _clientInformation.Email; }
            set
            {
                _clientInformation.Email = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get { return _clientInformation.Address; }
            set
            {
                _clientInformation.Address = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get { return _clientInformation.PhoneNumber; }
            set
            {
                _clientInformation.PhoneNumber = value;
                OnPropertyChanged();
            }
        }
    }
}
