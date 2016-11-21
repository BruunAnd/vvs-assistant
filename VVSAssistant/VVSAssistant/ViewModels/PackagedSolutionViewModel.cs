using System.Collections.ObjectModel;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionViewModel : ViewModelBase
    {
        // Base model
        private PackagedSolution _packagedSolution;

        public ObservableCollection<ApplianceViewModel> Appliances { get; private set; }

        public PackagedSolutionViewModel() : this(new PackagedSolution()) { }

        public PackagedSolutionViewModel(PackagedSolution packagedSolution)
        {
            _packagedSolution = packagedSolution;

            // Transform list of Appliance to list of ApplianceViewModel
            Appliances = new ObservableCollection<ApplianceViewModel>();
            foreach (var appliance in _packagedSolution.Appliances)
            {
                Appliances.Add(new ApplianceViewModel(appliance));
            }
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

        public string Description => string.Join(" ", Appliances);
    }
}
