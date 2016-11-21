using System.Collections.ObjectModel;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionViewModel : ViewModelBase
    {
        public PackagedSolutionViewModel()
        {
            Appliances = new ObservableCollection<ApplianceViewModel>();
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get; private set;
        }
    }
}
