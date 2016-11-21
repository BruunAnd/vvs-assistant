using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionViewModel : ViewModelBase
    {
        public PackagedSolutionViewModel()
        {
            Appliances = new ObservableCollection<ApplianceViewModel>();
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get; private set;
        }

        public void Push()
        {
            // Should call a method in the model to push the data in this object to the model / database.
            throw new NotImplementedException();
        }

        public void Pull()
        {
            // Should call a method in the model to pull data from the model / database to override this object.
            throw new NotImplementedException();
        }
    }
}
