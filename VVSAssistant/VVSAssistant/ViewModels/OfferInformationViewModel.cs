using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;
using System.Runtime.CompilerServices;

namespace VVSAssistant.ViewModels
{
    class OfferInformationViewModel : ViewModelBase
    {
        private readonly OfferInformation _offerInformation;

        public OfferInformationViewModel(OfferInformation offerInformation)
        {
            _offerInformation = offerInformation;
        }

        public int Id
        {
            get { return _offerInformation.Id; }
            set
            {
                _offerInformation.Id = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _offerInformation.Description; }
            set
            {
                _offerInformation.Description = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get { return _offerInformation.Price; }
            set
            {
                _offerInformation.Price = value;
                OnPropertyChanged();
            }
        }
    }
}
