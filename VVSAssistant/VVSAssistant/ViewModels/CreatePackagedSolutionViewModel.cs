using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Collections.Specialized;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Database;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;

namespace VVSAssistant.ViewModels
{
    class CreatePackagedSolutionViewModel : ViewModelBase
    {
        #region Property initializations

        private PackagedSolutionViewModel _packageSolution = new PackagedSolutionViewModel();
        public PackagedSolutionViewModel PackageSolution
        {
            get { return _packageSolution; }
        }
        #endregion

        #region Command initializations
        public RelayCommand AddApplianceToPackageSolution { get; }
        public RelayCommand RemoveApplianceFromPackageSolution { get; }
        public RelayCommand EditAppliance { get; }
        public RelayCommand RemoveAppliance { get; }
        public RelayCommand NewPackageSolution { get; }
        public RelayCommand SaveDialog { get; }
        #endregion

        #region Collections
        private readonly IDialogCoordinator _dialogCoordinator;

        public ObservableCollection<ApplianceViewModel> Appliances { get; }

        public FilterableListViewModel<ApplianceViewModel> FilterableApplianceList { get; private set; }
        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            Appliances = new ObservableCollection<ApplianceViewModel>();

            // Load list of appliances from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of Appliance to a list of ApplianceViewModel
                dbContext.Appliances.ToList().ForEach(a => Appliances.Add(new ApplianceViewModel(a)));
                // Create filtered list
                FilterableApplianceList = new FilterableListViewModel<ApplianceViewModel>(Appliances);
            }

            #region Command declarations
            AddApplianceToPackageSolution = new RelayCommand(x => 
            {
                var item = x as ApplianceViewModel;
                if (item != null) this.PackageSolution.Appliances.Add(item);
            });

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                var item = x as ApplianceViewModel;
                if (item != null) this.PackageSolution.Appliances.Remove(item);
            });

            /* EditAppliance = new RelayCommand(x =>
             {
                 var item = x as ApplianceViewModel;
                 if (item != null)
             });
             */

            RemoveAppliance = new RelayCommand(x =>
            {
                var appliance = x as ApplianceViewModel;
                if (appliance == null) return;
                Appliances.Remove(appliance);
                appliance.RemoveFromDatabase();
            });

            NewPackageSolution = new RelayCommand(x =>
            {
                if (this.PackageSolution.Appliances.Any()) this.PackageSolution.Appliances.Clear();
            }, x => this.PackageSolution.Appliances.Any());
            

            SaveDialog = new RelayCommand(x =>
            {
                RunCustomFromVm();
            });
            #endregion

            this.PackageSolution.Appliances.CollectionChanged += PackageSolutionAppliances_CollectionChanged;
        }

        private async void RunCustomFromVm()
        {
            var customDialog = new CustomDialog();

            var customDialogExampleContent = new SaveDialogViewModel("Gem pakkeløsning", "Navn:" ,instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                System.Diagnostics.Debug.WriteLine(instance.Input);
            });
            customDialog.Content = new SaveDialogView { DataContext = customDialogExampleContent };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackageSolution.NotifyCanExecuteChanged();
            SaveDialog.NotifyCanExecuteChanged();
        }
    }
}
