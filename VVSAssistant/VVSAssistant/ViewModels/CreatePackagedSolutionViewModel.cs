using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    public class CreatePackagedSolutionViewModel : FilterableViewModelBase<Appliance>
    {
        #region Property initializations
        private PackagedSolution _packagedSolution;
        public PackagedSolution PackagedSolution
        {
            get { return _packagedSolution; }
            set
            {
                _packagedSolution = value;
                AppliancesInSolution = new ObservableCollection<Appliance>();
                AppliancesInSolution.CollectionChanged += PackageSolutionAppliances_CollectionChanged;
            }

        }

        private readonly IDialogCoordinator _dialogCoordinator;

        private Appliance _selectedAppliance;
        public Appliance SelectedAppliance
        {
            get { return _selectedAppliance; }
            set
            {
                if (!SetProperty(ref _selectedAppliance, value)) return;

                // Notify if property was changed
                AddApplianceToPackageSolution.NotifyCanExecuteChanged();
                EditAppliance.NotifyCanExecuteChanged();
                RemoveAppliance.NotifyCanExecuteChanged();
            }
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
        public ObservableCollection<Appliance> Appliances { get; } = new ObservableCollection<Appliance>();
        public ObservableCollection<Appliance> AppliancesInSolution { get; set; }
        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            SetupFilterableView(Appliances);
            PackagedSolution = new PackagedSolution();
            _dialogCoordinator = dialogCoordinator;

            #region Command declarations

            AddApplianceToPackageSolution = new RelayCommand(x => 
            {
                AppliancesInSolution.Add(SelectedAppliance);
            }, x => SelectedAppliance != null);

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                AppliancesInSolution.Remove(SelectedAppliance);
            }, x => SelectedAppliance != null);

            EditAppliance = new RelayCommand(x =>
            {
                RunEditDialog();
            }, x => SelectedAppliance != null);

            RemoveAppliance = new RelayCommand(x =>
            {
                RemoveApplianceRENAMEMEPLZ(SelectedAppliance);
            }, x => SelectedAppliance != null);

            NewPackageSolution = new RelayCommand(x =>
            {
                AppliancesInSolution.Clear();
            }, x => PackagedSolution.Appliances.Any());

            SaveDialog = new RelayCommand(x =>
            {
                RunSaveDialog();
            }, x => AppliancesInSolution.Any());
            #endregion
        }

        private void RemoveApplianceRENAMEMEPLZ(Appliance appliance)
        {
            Appliances.Remove(appliance);

            DbContext.Appliances.Remove(appliance);
            DbContext.SaveChanges();
        }

        private void SaveCurrentPackagedSolution()
        {
            PackagedSolution.CreationDate = DateTime.Now;
            PackagedSolution.Appliances = new ApplianceList(AppliancesInSolution.ToList());

            DbContext.PackagedSolutions.Add(PackagedSolution);
            DbContext.SaveChanges();

            PackagedSolution = new PackagedSolution();
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

                    SaveCurrentPackagedSolution();
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

        public override void LoadDataFromDatabase()
        {
            DbContext.Appliances.ToList().ForEach(Appliances.Add);
        }

        protected override bool Filter(Appliance obj)
        {
            return true;
        }
    }
}
