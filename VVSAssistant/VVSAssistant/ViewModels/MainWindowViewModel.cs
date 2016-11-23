using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand(x =>
            {
                var str = x as string;
                OnNav(str);
            });
        }
        
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
        
        public RelayCommand NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            CurrentViewModel?.CloseDataConnection();

            switch (destination)
            {
                case ("ExistingPackagedSolutionView"):
                    CurrentViewModel = new ExistingPackagedSolutionsViewModel();
                    break;
                case ("CreatePackagedSolutionView"):
                    CurrentViewModel = new CreatePackagedSolutionViewModel(new DialogCoordinator());
                    break;
                case ("ExistingOffersView"):
                    CurrentViewModel = new ExistingOffersViewModel();
                    break;
                case ("CreateOfferView"):
                    CurrentViewModel = new CreateOfferViewModel(new DialogCoordinator());
                    break;
                default:
                    CurrentViewModel = null;
                    return; // Don't open dataconnection
            }

            CurrentViewModel?.OpenDataConnection();
            CurrentViewModel?.Initialize();
        }
    }
}
