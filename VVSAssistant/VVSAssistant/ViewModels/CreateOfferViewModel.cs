using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Exceptions;
using VVSAssistant.Models;
using System.Reflection;

namespace VVSAssistant.ViewModels
{
    class CreateOfferViewModel : ViewModelBase
    {
        public RelayCommand CreateNewOffer { get; }

        /* Packaged solutions on list */ 
        private ObservableCollection<PackagedSolutionViewModel> _packagedSolutions;
        private ObservableCollection<ClientViewModel> _clients;

        private OfferViewModel _offer;

        public CreateOfferViewModel()
        {
            _offer = new OfferViewModel(new Offer());
            CreateNewOffer = new RelayCommand(x => CreateOffer(), x => VerifyNeededInformation());
        }

        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions
        {
            get { return _packagedSolutions; }
            set
            {
                _packagedSolutions = value;
                OnPropertyChanged();
            }
        }

        public OfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; }
        }

        public void CreateOffer()
        {
            /* Call the exporter class, export the pdf. Save the pdf in the system. */
        }

        /// <summary>
        /// Uses reflection to check whether or not any of the properties in the passed object is null or empty.
        /// </summary>
        /// <returns></returns>
        private bool IsPropertyNullOrEmpty(object objectToCheck)
        {
            //TODO: Fix this method. Doesn't work for some reason, don't know why. 
            /* Fetch all properties */
            foreach (PropertyInfo pi in objectToCheck.GetType().GetProperties())
            {
                string value = (string) pi.GetValue(objectToCheck);
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if both Client and Packaged Solution has a name
        /// </summary>
        /// <returns></returns>
        public bool VerifyNeededInformationDeprecated()
        {
            if (IsPropertyNullOrEmpty(Offer.Client.ClientInformation) ||
                IsPropertyNullOrEmpty(Offer.Client)                   ||
                IsPropertyNullOrEmpty(Offer.PackagedSolution)         ||
                IsPropertyNullOrEmpty(Offer))
                return false;
            else
                return true;
        }

        public bool VerifyNeededInformation()
        {
            if (Offer.Client.ClientInformation.Name == null ||
                Offer.PackagedSolution.Name         == null ||
                Offer.OfferInformation              == null )
            {
                return false;
            }
            else
                return true;
        }
    }
}
