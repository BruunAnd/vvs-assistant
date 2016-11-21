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
        /* Packaged solutions on list */ 
        private ObservableCollection<PackagedSolutionViewModel> _packagedSolutions;

        #region Temporary data for creating an offer
        private PackagedSolution _currentPackagedSolution;
        private Salary _currentSalary;
        #endregion

        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions
        {
            get { return _packagedSolutions; }
            set
            {
                _packagedSolutions = value;
                OnPropertyChanged();
            }
        }

        //TODO: We should probably add this method to this class in the component design chapter - Can't see it anywhere else
        public Offer CreateOffer()
        {
            return null;
        }
    }
}
