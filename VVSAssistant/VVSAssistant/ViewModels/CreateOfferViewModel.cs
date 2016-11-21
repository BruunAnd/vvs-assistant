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
        }

        public void CreateOffer()
        {
            /* Call the exporter class, export the pdf. Save the pdf in the system. */
        }

        /// <summary>
        /// Uses reflection to check whether or not any of the properties in the passed object is null or empty.
        /// </summary>
        /// <param name="objectToCheck"></param>
        /// <returns></returns>
        private bool IsPropertyNullOrEmpty(object objectToCheck)
        {
            /* Fetch all properties */ 
            foreach (PropertyInfo pi in objectToCheck.GetType().GetProperties())
            {
                /* Make sure that it is a property with a name */
                if (pi.PropertyType == typeof(string))
                {
                    /* Fetch the value of the property, and see if it is null or empty */
                    string value = (string)pi.GetValue(objectToCheck);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if ALL needed information in the offer is present. If any information is missing, return false.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        private bool VerifyNeededInformation()
        {
            if (IsPropertyNullOrEmpty(Offer.Client.ClientInformation) == false &&
                IsPropertyNullOrEmpty(Offer.Client) == false &&
                IsPropertyNullOrEmpty(Offer.PackagedSolution) == false &&
                IsPropertyNullOrEmpty(Offer) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
