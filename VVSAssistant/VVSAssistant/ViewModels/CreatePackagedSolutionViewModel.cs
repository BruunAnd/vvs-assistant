using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.ViewModels
{
    public class CreatePackagedSolutionViewModel : FilterableViewModelBase<Appliance>
    {
        #region Property initializations

        private bool _includeHeatPumps;
        public bool IncludeHeatPumps
        {
            get { return _includeHeatPumps; }
            set
            {
                if (SetProperty(ref _includeHeatPumps, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeBoilers;
        public bool IncludeBoilers
        {
            get { return _includeBoilers; }
            set
            {
                if (SetProperty(ref _includeBoilers, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeTemperatureControllers;
        public bool IncludeTemperatureControllers
        {
            get { return _includeTemperatureControllers; }
            set
            {
                if (SetProperty(ref _includeTemperatureControllers, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeSolarPanels;
        public bool IncludeSolarPanels
        {
            get { return _includeSolarPanels; }
            set
            {
                if (SetProperty(ref _includeSolarPanels, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeContainers;
        public bool IncludeContainers
        {
            get { return _includeContainers; }
            set
            {
                if (SetProperty(ref _includeContainers, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeLowTempHeatPumps;
        public bool IncludeLowTempHeatPumps
        {
            get { return _includeLowTempHeatPumps; }
            set
            {
                if (SetProperty(ref _includeLowTempHeatPumps, value))
                    FilteredCollectionView.Refresh();
            }
        }

        private bool _includeCentralHeatingPlants;
        public bool IncludeCentralHeatingPlants
        {
            get { return _includeCentralHeatingPlants; }
            set
            {
                if (SetProperty(ref _includeCentralHeatingPlants, value))
                    FilteredCollectionView.Refresh();
            }
        }

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
                AddApplianceToPackagedSolution.NotifyCanExecuteChanged();
                EditAppliance.NotifyCanExecuteChanged();
                RemoveAppliance.NotifyCanExecuteChanged();
            }
        }
        #endregion

        #region Command initializations
        public RelayCommand AddApplianceToPackagedSolution { get; }
        public RelayCommand RemoveApplianceFromPackagedSolution { get; }
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

            AddApplianceToPackagedSolution = new RelayCommand(x =>
            {
                if (SelectedAppliance.Type == ApplianceTypes.Boiler)
                    RunAddBoilerDialog(SelectedAppliance);
                else
                    AddApplianceToSolution(SelectedAppliance);
            }, x => SelectedAppliance != null);

            RemoveApplianceFromPackagedSolution = new RelayCommand(x =>
            {
                if (PackagedSolution.PrimaryHeatingUnit == SelectedAppliance)
                    PackagedSolution.PrimaryHeatingUnit = null;
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

        private void AddApplianceToSolution(Appliance appliance)
        {
            AppliancesInSolution.Add(appliance);
        }

        private void RemoveApplianceRENAMEMEPLZ(Appliance appliance)
        {
            Appliances.Remove(appliance);

            DbContext.Appliances.Remove(appliance);
            DbContext.SaveChanges();
        }

        private void SaveCurrentPackagedSolution()
        {
            /* IMPORTANT 
             * A packaged solution should not be saved if there exists
             *  a solar collector without a container tied to it. */

            PackagedSolution.CreationDate = DateTime.Now;
            PackagedSolution.Appliances = new ApplianceList(AppliancesInSolution.ToList());

            DbContext.PackagedSolutions.Add(PackagedSolution);
            DbContext.SaveChanges();

            PackagedSolution = new PackagedSolution();
        }

        private async void RunAddBoilerDialog(Appliance appliance)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new AddBoilerDialogViewModel("Tilføj kedel", "Vælg om den nye kedel skal være primær- eller sekundærkedel.",
                instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog), async instanceCompleted =>
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                    AddApplianceToSolution(appliance);
                    if (!instanceCompleted.IsPrimaryBoiler) return;
                    if (PackagedSolution.PrimaryHeatingUnit != null)
                    {
                        // Inform the user that their previous primary heating unit will be replaced
                        await _dialogCoordinator.ShowMessageAsync(this, "Information",
                                $"Da du har valgt en ny primærkedel er komponentet {PackagedSolution.PrimaryHeatingUnit.Name} nu en sekundærkedel.");
                    }
                    PackagedSolution.PrimaryHeatingUnit = appliance;
                });

            customDialog.Content = new AddBoilerDialogView() { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void RunSolarContainerDialog(Appliance appliance, ObservableCollection<Appliance> appliances)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SolarContainerDialogViewModel("Er denne behol", "Vælg om den nye kedel skal være primær- eller sekundærkedel.",
                instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                instanceCompleted =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                    AddApplianceToSolution(appliance);
                    if (instanceCompleted.IsPrimaryBoiler)
                        PackagedSolution.PrimaryHeatingUnit = appliance;
                });

            customDialog.Content = new AddBoilerDialogView() { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
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
            // Filter based on type first
            if (IncludeBoilers ||
                IncludeCentralHeatingPlants ||
                IncludeContainers ||
                IncludeHeatPumps ||
                IncludeLowTempHeatPumps ||
                IncludeSolarPanels ||
                IncludeTemperatureControllers)
            {
                switch (obj.Type)
                {
                    case ApplianceTypes.Boiler:
                        if (!IncludeBoilers)
                            return false;
                        break;
                    case ApplianceTypes.CHP:
                        if (!IncludeCentralHeatingPlants)
                            return false;
                        break;
                    case ApplianceTypes.Container:
                        if (!IncludeContainers)
                            return false;
                        break;
                    case ApplianceTypes.HeatPump:
                        if (!IncludeHeatPumps)
                            return false;
                        break;
                    case ApplianceTypes.LowTempHeatPump:
                        if (!IncludeLowTempHeatPumps)
                            return false;
                        break;
                    case ApplianceTypes.SolarPanel:
                        if (!IncludeSolarPanels)
                            return false;
                        break;
                    case ApplianceTypes.TemperatureController:
                        if (!IncludeTemperatureControllers)
                            return false;
                        break;
                    default:
                        return false;
                }
            }

            // Filter based on FilterString
            return obj.Name.ContainsIgnoreCase(FilterString) || obj.Description.ContainsIgnoreCase(FilterString);
        }

        private void HandleAddApplianceToPackagedSolution(Appliance appToAdd)
        {
            if (appToAdd.DataSheet is HeatingUnitDataSheet &&
                PackagedSolution.PrimaryHeatingUnit == null)
            {
                //TODO: Implement the comment below: 
                /* Prompt user for whether or not the heating unit is primary */
                /* If it is primary, ask whether or not it is for water heating, 
                 * room heating, or both. */
            }

            if (appToAdd.DataSheet is ContainerDataSheet &&
                PackagedSolution.Appliances.ContainsWhere(a => a.DataSheet is SolarCollectorDataSheet))
            {
                /* Prompt the user for whether or not the container is tied to any of the solar collector. */
            }

            if (appToAdd.DataSheet is SolarCollectorDataSheet &&
                PackagedSolution.Appliances.ContainsWhere(a => a.DataSheet is ContainerDataSheet))
            {
                /* Prompt the user for whether or not any of the containers are tied to the solar collector */
            }
        }
    }
}
