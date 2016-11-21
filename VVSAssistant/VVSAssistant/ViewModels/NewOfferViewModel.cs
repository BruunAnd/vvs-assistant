using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Exceptions;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    class NewOfferViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<PackagedSolutionViewModel> _packagedSolutions;
        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions
        {
            get
            {
                if (_packagedSolutions != null && _packagedSolutions.Count != 0)
                    return _packagedSolutions;
                else
                    throw new NullReferenceException("You must add packaged solutions before composing a new offer. ");
            }

            set
            {
                //TODO: Test whether or not this fails when value is just a single PackagedSolution (it shouldn't)
                if (value is ObservableCollection<PackagedSolutionViewModel>)
                {
                    _packagedSolutions.Concat(value);
                    OnPropertyChanged();
                }
                else
                {
                    throw new InvalidParameterTypeException("The type of objects added to list of packaged solutions can only be PackagedSolutionViewModel. ");
                }
            }
        }

        private string _offerDescription;
        public string OfferDescription
        {
            get { return _offerDescription; }
            set
            {
                _offerDescription = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //TODO: We should probably add this method to this class in the component design chapter - Can't see it anywhere else
        public Offer CreateOffer()
        {

        }
    }
}
