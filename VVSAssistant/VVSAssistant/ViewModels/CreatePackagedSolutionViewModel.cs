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
    public class CreatePackagedSolutionViewModel : ViewModelBase
    {
        #region Property initializations
        public PackagedSolutionViewModel PackagedSolution { get; } = new PackagedSolutionViewModel();

        private IDialogCoordinator _dialogCoordinator;
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
        public ObservableCollection<ApplianceViewModel> Appliances { get; } = new ObservableCollection<ApplianceViewModel>();

        public FilterableListViewModel<ApplianceViewModel> FilterableApplianceList { get; private set; }
        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

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
                if (item != null) this.PackagedSolution.Appliances.Add(item);
            });

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                var item = x as ApplianceViewModel;
                if (item != null) this.PackagedSolution.Appliances.Remove(item);
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
                this.PackagedSolution.Appliances.Clear();
            }, x => this.PackagedSolution.Appliances.Any());
            

            SaveDialog = new RelayCommand(x =>
            {
                RunSaveDialog();
            }, x => this.PackagedSolution.Appliances.Any());
            #endregion

            this.PackagedSolution.Appliances.CollectionChanged += PackageSolutionAppliances_CollectionChanged;
        }

        private async void RunSaveDialog()
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SaveDialogViewModel("Gem pakkeløsning", "Navn:", _dialogCoordinator, instance =>
            {
                // Makes it possible to close the dialog.
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });
            dialogViewModel.DialogFilled += SaveDialogFilled;
            customDialog.Content = new SaveDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void SaveDialogFilled(string input)
        {
            PackagedSolution.Name = input;
            PackagedSolution.SaveToDatabase();
            await _dialogCoordinator.ShowMessageAsync(this, "Gemt", "Din pakkeløsning blev gemt under navnet " + input);
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackageSolution.NotifyCanExecuteChanged();
            SaveDialog.NotifyCanExecuteChanged();
        }
    }
}
