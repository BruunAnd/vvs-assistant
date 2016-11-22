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

        public string Intro
        {
            get { return _offerInformation.Intro; }
            set
            {
                _offerInformation.Intro = value;
                OnPropertyChanged();
            }
        }

        public string Outro
        {
            get { return _offerInformation.Outro; }
            set
            {
                _offerInformation.Outro = value;
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

        public string Title
        {
            get { return _offerInformation.Title; } 
            set { _offerInformation.Title = value; OnPropertyChanged(); }
        }

        public bool ApplyTax
        {
            get { return _offerInformation.ApplyTax; }
            set { _offerInformation.ApplyTax = value; OnPropertyChanged(); }
        }
    }
}
