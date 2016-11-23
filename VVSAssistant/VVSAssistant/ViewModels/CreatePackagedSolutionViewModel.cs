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
        public ApplianceViewModel SelectedAppliance { get; set; }
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
                PackagedSolution.Appliances.Add(SelectedAppliance);
            }, x => SelectedAppliance != null);

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                PackagedSolution.Appliances.Remove(SelectedAppliance);
            }, x => SelectedAppliance != null);

            EditAppliance = new RelayCommand(x =>
             {
                 RunEditDialog();
             });

            RemoveAppliance = new RelayCommand(x =>
            {
                SelectedAppliance.RemoveFromDatabase();
                
                Appliances.Remove(SelectedAppliance);
            }, x => SelectedAppliance != null);

            NewPackageSolution = new RelayCommand(x =>
            {
                PackagedSolution.Appliances.Clear();
            }, x => PackagedSolution.Appliances.Any());
            

            SaveDialog = new RelayCommand(x =>
            {
                RunSaveDialog();
            }, x => PackagedSolution.Appliances.Any());
            #endregion

            PackagedSolution.Appliances.CollectionChanged += PackageSolutionAppliances_CollectionChanged;
        }

        private async void RunSaveDialog()
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SaveDialogViewModel("Gem pakkeløsning", "Navn:",
                instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                instanceCompleted =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    PackagedSolution.Name = instanceCompleted.Input;
                    PackagedSolution.SaveToDatabase();
                }); 
            
            customDialog.Content = new SaveDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void RunEditDialog()
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new EditApplianceViewModel(SelectedAppliance, instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog), instanceCompleted => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));

            customDialog.Content = new EditApplianceView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }
        
        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackageSolution.NotifyCanExecuteChanged();
            SaveDialog.NotifyCanExecuteChanged();
        }
    }
}
