using System.Collections.ObjectModel;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionViewModel : ViewModelBase
    {
        private PackagedSolution _packagedSolution;

        public PackagedSolutionViewModel()
        {
            _packagedSolution = new PackagedSolution();
            Appliances = new ObservableCollection<ApplianceViewModel>();
            // TODO: Add constructor for existing packaged solution object?
        }

        public string Name
        {
            get { return _packagedSolution.Name; }
            set
            {
                if (_packagedSolution.Name == value) return;
                _packagedSolution.Name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get; private set;
        }
    }
}
