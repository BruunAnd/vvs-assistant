using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Database;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionsViewModel : ViewModelBase
    {
        public PackagedSolutionsViewModel()
        {
            Appliances = new ObservableCollection<ViewModels.ApplianceViewModel>();
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
