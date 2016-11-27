using System;
using System.Collections.Generic;
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

        private bool _includeSolarStations;
        public bool IncludeSolarStations
        {
            get { return _includeSolarStations; }
            set
            {
                if (SetProperty(ref _includeSolarStations, value))
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
                AddApplianceToPackagedSolutionCommand.NotifyCanExecuteChanged();
                EditApplianceCommand.NotifyCanExecuteChanged();
                RemoveApplianceCommand.NotifyCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command initializations
        public RelayCommand AddApplianceToPackagedSolutionCommand { get; }
        public RelayCommand RemoveApplianceFromPackagedSolutionCommand { get; }
        public RelayCommand EditApplianceCommand { get; }
        public RelayCommand RemoveApplianceCommand { get; }
        public RelayCommand NewPackagedSolutionCommand { get; }
        public RelayCommand SaveDialog { get; }
        public RelayCommand CreateNewAppliance { get; }
        #endregion

        #region Collections
        public ObservableCollection<Appliance> Appliances { get; } = new ObservableCollection<Appliance>();
        public ObservableCollection<Appliance> AppliancesInSolution { get; set; } = new ObservableCollection<Appliance>();
        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            SetupFilterableView(Appliances);
            PackagedSolution = new PackagedSolution();
            _dialogCoordinator = dialogCoordinator;

            #region Command declarations

            AddApplianceToPackagedSolutionCommand = new RelayCommand(
                x => HandleAddApplianceToPackagedSolution(SelectedAppliance), 
                x => SelectedAppliance != null);

            RemoveApplianceFromPackagedSolutionCommand = new RelayCommand(x =>
            {
                if (PackagedSolution.PrimaryHeatingUnit == SelectedAppliance)
                    PackagedSolution.PrimaryHeatingUnit = null;
                AppliancesInSolution.Remove(SelectedAppliance);
            }, x => SelectedAppliance != null);

            EditApplianceCommand = new RelayCommand(x =>
            {
                RunEditDialog();
            }, x => SelectedAppliance != null);

            RemoveApplianceCommand = new RelayCommand(x =>
            {
                RemoveApplianceRENAMEMEPLZ(SelectedAppliance);
            }, x => SelectedAppliance != null);

            NewPackagedSolutionCommand = new RelayCommand(x =>
            {
                CreateNewPackagedSolution();
            }, x => AppliancesInSolution.Any());

            SaveDialog = new RelayCommand(x =>
            {
                RunSaveDialog();
            }, x => AppliancesInSolution.Any());

            CreateNewAppliance = new RelayCommand(x => 
            {
                RunCreateApplianceDialog();
            }
            );
            #endregion
        }

        private async void CreateNewPackagedSolution()
        {
            if (AppliancesInSolution.Any())
            {
                var result = await _dialogCoordinator.ShowMessageAsync(this, "Ny pakkeløsning",
                                "Hvis du opretter en ny pakkeløsning mister du arbejdet på din nuværende pakkeløsning. Vil du fortsætte?",
                                MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Negative)
                    return;
            }

            PackagedSolution = new PackagedSolution();
            AppliancesInSolution.Clear();
        }

        private void AddApplianceToSolution(Appliance appliance)
        {
            AppliancesInSolution.Add(appliance);
        }

        private async void RemoveApplianceRENAMEMEPLZ(Appliance appliance)
        {
            // Check if the appliance is used in any packaged solutions
            var conflictingSolutions = DbContext.PackagedSolutions.Where(s => s.ApplianceInstances.Any(a => a.Appliance.Id == appliance.Id)).ToList();
            if (conflictingSolutions.Count > 0)
            {
                var formattedSolutionString = string.Join("\n", conflictingSolutions.Select(x => $"- {x.Name}"));
                await _dialogCoordinator.ShowMessageAsync(this, "Fejl",
                    $"Komponentet kan ikke slettes, da det findes i følgende pakkeløsninger:\n{formattedSolutionString}");
                return;
            }

            // Remove from current solution
            if (AppliancesInSolution.Contains(appliance))
                AppliancesInSolution.Remove(appliance);

            // Remove from visual list of appliances
            Appliances.Remove(appliance);

            // Remove from database
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

        private async void RunAddHeatingUnitDialog(Appliance appliance)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new AddHeatingUnitDialogViewModel(instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                async instanceCompleted =>
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                    AddApplianceToSolution(appliance);
                    if (!instanceCompleted.IsPrimaryBoiler) return;
                    if (PackagedSolution.PrimaryHeatingUnit != null)
                    {
                        // Inform the user that their previous primary heating unit will be replaced
                        await _dialogCoordinator.ShowMessageAsync(this, "Information",
                                $"Da du har valgt en ny primærkedel er komponentet {PackagedSolution.PrimaryHeatingUnit.Name} nu en sekundærkedel.");
                        // todo set purpose of heating unit
                    }
                    PackagedSolution.PrimaryHeatingUnit = appliance;
                });

            customDialog.Content = new AddHeatingUnitDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void RunSolarContainerDialog(string message, string title, Appliance appliance, ObservableCollection<Appliance> appliances)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SolarContainerDialogViewModel(message, title, appliance, appliances, AppliancesInSolution, PackagedSolution,
                closeHandler => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                completionHandler => _dialogCoordinator.HideMetroDialogAsync(this, customDialog) );

            customDialog.Content = new SolarContainerDialogView { DataContext = dialogViewModel };

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

            var dialogViewModel = new CreateApplianceDialogViewModel(SelectedAppliance, false,
                                      instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                                      instanceCompleted =>
                                      {
                                          _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                                          foreach (var appliance in Appliances)
                                              FilteredCollectionView.Refresh();
                                      });

            customDialog.Content = new CreateApplianceDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
            
        }

        private async void RunCreateApplianceDialog()
        {
            var customDialog = new CustomDialog();
            var newAppliance = new Appliance();
            var dialogViewModel = new CreateApplianceDialogViewModel(newAppliance, true,
                closeHandler => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                completionHandler => 
                {
                    DbContext.Appliances.Add(newAppliance);
                    Appliances.Add(newAppliance);
                    AppliancesInSolution.Add(newAppliance);
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

            customDialog.Content = new CreateApplianceDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackagedSolutionCommand.NotifyCanExecuteChanged();
            SaveDialog.NotifyCanExecuteChanged();
        }

        public override void LoadDataFromDatabase()
        {
            DbContext.Appliances.ToList().ForEach(Appliances.Add);
        }

        protected override bool Filter(Appliance obj)
        {
            // Filter based on type first
            if (IncludeBoilers || IncludeCentralHeatingPlants || IncludeContainers || IncludeHeatPumps
                || IncludeLowTempHeatPumps || IncludeSolarPanels || IncludeTemperatureControllers)
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
                    case ApplianceTypes.SolarStation:
                        if (!IncludeSolarStations)
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
            if (appToAdd.DataSheet is HeatingUnitDataSheet)
            {
                /* Prompt user for whether or not the heating unit is primary */
                /* If it is primary, ask whether or not it is for water heating, 
                 * room heating, or both. */
                RunAddHeatingUnitDialog(appToAdd);
            }
            else if (appToAdd.DataSheet is ContainerDataSheet &&
                AppliancesInSolution.Any(a => a.DataSheet is SolarCollectorDataSheet))
            {
                /* Prompt the user for whether or not the container is tied to any of the solar collector. */
                var title = "Vælg solfangeren som denne beholder er forbundet til";
                var message = "Hvis beholderen ikke er forbundet til en solfanger, tryk på \"Acceptér\"";
                var appliances = new ObservableCollection<Appliance>(AppliancesInSolution.Where
                                                 (a => a.DataSheet is SolarCollectorDataSheet));
                RunSolarContainerDialog(message, title, appToAdd, appliances);
            }
            else if (appToAdd.DataSheet is SolarCollectorDataSheet &&
                AppliancesInSolution.Any(a => a.DataSheet is ContainerDataSheet))
            {
                /* Prompt the user for whether or not any of the containers are tied to the solar collector */
                var title = "Vælg beholderen som denne solfanger er forbundet til ";
                var message = "Hvis solfangeren ikke er forbundet til en beholder, tryk på \"Acceptér\"";
                var appliances = new ObservableCollection<Appliance>(AppliancesInSolution.Where
                                                 (a => a.DataSheet is ContainerDataSheet));
                RunSolarContainerDialog(message, title, appToAdd, appliances);
            }
            else
            {
                Console.WriteLine("All checks passed, adding to packaged solution with no window.");
                AddApplianceToSolution(appToAdd);
            }
        }
    }
}
