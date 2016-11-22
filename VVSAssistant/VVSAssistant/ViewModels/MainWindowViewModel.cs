using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;

namespace VVSAssistant.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        
        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand(x =>
            {
                string str = x as string;
                OnNav(str);
            });
        }
        
        private ExistingPackagedSolutionsViewModel _existingPackagedSolutionViewModel = new ExistingPackagedSolutionsViewModel();
        private CreatePackagedSolutionViewModel _createPackagedSolutionViewModel = new CreatePackagedSolutionViewModel(new DialogCoordinator());
        private ExistingOffersViewModel _existingOffersViewModel = new ExistingOffersViewModel();
        private CreateOfferViewModel _createOfferViewModel = new CreateOfferViewModel(new DialogCoordinator());

        private ViewModelBase _CurrentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }
        
        public RelayCommand NavCommand { get; private set; }

        private void OnNav(string destination)
        {

            switch (destination)
            {
                case ("ExistingPackagedSolutionView"):
                    CurrentViewModel = _existingPackagedSolutionViewModel;
                    break;
                case ("CreatePackagedSolutionView"):
                    CurrentViewModel = _createPackagedSolutionViewModel;
                    break;
                case ("ExistingOffersView"):
                    CurrentViewModel = _existingOffersViewModel;
                    break;
                case ("CreateOfferView"):
                    CurrentViewModel = _createOfferViewModel;
                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }
    }
}
