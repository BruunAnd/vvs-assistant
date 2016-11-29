using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Extensions;
using VVSAssistant.Functions;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.Functions.Calculation;

namespace VVSAssistant.ViewModels
{
    public class CreatePackagedSolutionViewModel : FilterableViewModelBase<Appliance>
    {
        #region Inclusion properties

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

        private bool _includeWaterHeaters;
        public bool IncludeWaterHeaters
        {
            get { return _includeWaterHeaters; }
            set
            {
                if (SetProperty(ref _includeWaterHeaters, value))
                    FilteredCollectionView.Refresh();
            }
        }

        #endregion

        #region Base properties

        public PackagedSolution PackagedSolution { get; set; }
        private readonly IDialogCoordinator _dialogCoordinator;

        private Appliance _selectedAppliance;
        public Appliance SelectedAppliance
        {
            get { return _selectedAppliance; }
            set
            {
                if (!SetProperty(ref _selectedAppliance, value)) return;

                // Notify if property was changed
                AddApplianceToPackagedSolutionCmd?.NotifyCanExecuteChanged();
                EditApplianceCmd?.NotifyCanExecuteChanged();
                RemoveApplianceCmd?.NotifyCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string _eeiResult;
        public string EEIResultsRoomHeating
        {
            get { return _eeiResult; }
            set { _eeiResult = value; OnPropertyChanged(); }
        }

        private string _eeiResultSecondary;
        public string EEIResultsWaterHeating
        {
            get { return _eeiResultSecondary; }
            set { _eeiResultSecondary = value; OnPropertyChanged(); }
        }

        private readonly CalculationManager _calculationManager;
        
        #endregion

        #region Commands

        public RelayCommand AddApplianceToPackagedSolutionCmd { get; }
        public RelayCommand RemoveApplianceFromPackagedSolutionCmd { get; }
        public RelayCommand EditApplianceCmd { get; }
        public RelayCommand RemoveApplianceCmd { get; }
        public RelayCommand NewPackagedSolutionCmd { get; }
        public RelayCommand SavePackagedSolutionCmd { get; }
        public RelayCommand CreateNewApplianceCmd { get; }
        public RelayCommand PdfExportCmd { get; }

        #endregion

        #region Collections

        public ObservableCollection<Appliance> Appliances { get; }
        public ObservableCollection<Appliance> AppliancesInPackagedSolution { get; set; }

        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            #region Initialize collections

            Appliances = new ObservableCollection<Appliance>();
            SetupFilterableView(Appliances);
            AppliancesInPackagedSolution = new ObservableCollection<Appliance>();

            #endregion

            #region Initialize properties

            _calculationManager = new CalculationManager();
            _dialogCoordinator = dialogCoordinator;
            PackagedSolution = new PackagedSolution();

            #endregion

            #region Bind event listeners

            AppliancesInPackagedSolution.CollectionChanged += PackageSolutionAppliances_CollectionChanged;

            #endregion

            #region Command declarations

            AddApplianceToPackagedSolutionCmd = new RelayCommand(
                x => HandleAddApplianceToPackagedSolution(SelectedAppliance),
                x => SelectedAppliance != null);

            RemoveApplianceFromPackagedSolutionCmd = new RelayCommand(x =>
            {
                if (PackagedSolution.PrimaryHeatingUnit == SelectedAppliance)
                    PackagedSolution.PrimaryHeatingUnit = null;
                
                AppliancesInPackagedSolution.Remove(SelectedAppliance);
                
            }, x => SelectedAppliance != null);

            EditApplianceCmd = new RelayCommand(x =>
            {
                RunEditDialog();
            }, x => SelectedAppliance != null);

            RemoveApplianceCmd = new RelayCommand(x =>
            {
                DropAppliance(SelectedAppliance);
            }, x => SelectedAppliance != null);

            NewPackagedSolutionCmd = new RelayCommand(x =>
            {
                CreateNewPackagedSolution();
            }, x => AppliancesInPackagedSolution.Any());

            SavePackagedSolutionCmd = new RelayCommand(x =>
            {
                if (string.IsNullOrEmpty(PackagedSolution.Name))
                    RunSaveDialog();
                else
                    SaveCurrentPackagedSolution();
            }, x => AppliancesInPackagedSolution.Any());

            CreateNewApplianceCmd = new RelayCommand(x =>
            {
                RunCreateApplianceDialog();
            }
            );
            PdfExportCmd = new RelayCommand(x =>
            {
                PackagedSolution.Appliances = new ApplianceList(AppliancesInPackagedSolution.ToList());
                var e = new Exporter();
                e.ExportEnergyLabel(PackagedSolution);
            });
            #endregion
        }

        #region Methods


        private async void CreateNewPackagedSolution()
        {
            if (!AppliancesInPackagedSolution.Any()) return;

            var result = await _dialogCoordinator.ShowMessageAsync(this, "Ny pakkeløsning",
                "Hvis du opretter en ny pakkeløsning mister du arbejdet på din nuværende pakkeløsning. Vil du fortsætte?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Negative) return;

            AppliancesInPackagedSolution.Clear();
            PackagedSolution = new PackagedSolution();
        }

        private void AddApplianceToPackagedSolution(Appliance appliance)
        {
            AppliancesInPackagedSolution.Add(appliance);
        }

        /// <summary>
        /// Drops an appliance from the database and removes it from the displayed list in the UI
        /// </summary>
        /// <param name="appliance"></param>
        private async void DropAppliance(Appliance appliance)
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
            if (AppliancesInPackagedSolution.Contains(appliance))
                AppliancesInPackagedSolution.Remove(appliance);

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

            var packSol = DbContext.PackagedSolutions.SingleOrDefault(p => p.Id == PackagedSolution.Id);
            if (packSol != null) //It's already in the DB
            {
                PackagedSolution.Appliances.Clear();
                AppliancesInPackagedSolution.ToList().ForEach(appliance => PackagedSolution.Appliances.Add(appliance));
                DbContext.Entry(packSol).CurrentValues.SetValues(PackagedSolution);
            }
            else //Hasn't been saved yet
            {
                PackagedSolution.CreationDate = DateTime.Now;
                PackagedSolution.Appliances = new ApplianceList(AppliancesInPackagedSolution.ToList());
                DbContext.PackagedSolutions.Add(PackagedSolution);
            }

            DbContext.SaveChanges();
        }

        private async void RunAddHeatingUnitDialog(Appliance appliance)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new AddHeatingUnitDialogViewModel(instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                async instanceCompleted =>
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                    if (instanceCompleted.IsPrimaryBoiler)
                    {
                        if (PackagedSolution.PrimaryHeatingUnit != null)
                        {
                            // Inform the user that their previous primary heating unit will be replaced
                            await _dialogCoordinator.ShowMessageAsync(this, "Information",
                                    $"Da du har valgt en ny primærkedel er komponentet {PackagedSolution.PrimaryHeatingUnit.Name} nu en sekundærkedel.");
                            // todo set purpose of heating unit   
                        }

                        (appliance.DataSheet as HeatingUnitDataSheet).isRoomHeater = instanceCompleted.IsUsedForRoomHeating ? true : false;
                        (appliance.DataSheet as HeatingUnitDataSheet).isWaterHeater = instanceCompleted.IsUsedForWaterHeating ? true : false;
                        PackagedSolution.PrimaryHeatingUnit = appliance;
                    }
                    AddApplianceToPackagedSolution(appliance);
                });

            customDialog.Content = new AddHeatingUnitDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void RunSolarContainerDialog(string message, string title, Appliance appliance, ObservableCollection<Appliance> appliances)
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SolarContainerDialogViewModel(message, title, appliance, appliances, AppliancesInPackagedSolution, PackagedSolution,
                                                                    closeHandler => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                                                                    completionHandler => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));

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
                                          OnPropertyChanged("PackagedSolution.Name");
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
                                          _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                                      });

            customDialog.Content = new CreateApplianceDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEei();
            NewPackagedSolutionCmd.NotifyCanExecuteChanged();
            SavePackagedSolutionCmd.NotifyCanExecuteChanged();
        }

        public override  void LoadDataFromDatabase()
        {
            DbContext.Appliances.ToList().ForEach(Appliances.Add);
        }

        public void LoadExistingPackagedSolution(int packagedSolutionId)
        {
            var existingPackagedSolution = DbContext.PackagedSolutions.FirstOrDefault(p => p.Id == packagedSolutionId);
            if (existingPackagedSolution == null) return;

            // Copy primary heating unit
            PackagedSolution.PrimaryHeatingUnit = existingPackagedSolution.PrimaryHeatingUnit;

            // Copy solarcontainers
            foreach (var solarContainer in existingPackagedSolution.SolarContainers)
                PackagedSolution.SolarContainers.Add(solarContainer);
            
            // Copy appliances
            foreach (var appliance in existingPackagedSolution.Appliances)
                AppliancesInPackagedSolution.Add(appliance);
        }

        protected override bool Filter(Appliance obj)
        {
            // Filter based on type first
            if (IncludeBoilers || IncludeCentralHeatingPlants || IncludeContainers || IncludeHeatPumps
                || IncludeLowTempHeatPumps || IncludeSolarPanels || IncludeTemperatureControllers || IncludeWaterHeaters)
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
                    case ApplianceTypes.WaterHeater:
                        if (!IncludeWaterHeaters)
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
                AppliancesInPackagedSolution.Any(a => a.DataSheet is SolarCollectorDataSheet))
            {
                /* Prompt the user for whether or not the container is tied to any of the solar collector. */
                var title = "Vælg solfangeren som denne beholder er forbundet til";
                var message = "Hvis beholderen ikke er forbundet til en solfanger, tryk på \"Acceptér\"";
                var appliances = new ObservableCollection<Appliance>(AppliancesInPackagedSolution.Where
                                                 (a => a.DataSheet is SolarCollectorDataSheet));
                RunSolarContainerDialog(message, title, appToAdd, appliances);
            }
            else if (appToAdd.DataSheet is SolarCollectorDataSheet &&
                AppliancesInPackagedSolution.Any(a => a.DataSheet is ContainerDataSheet))
            {
                /* Prompt the user for whether or not any of the containers are tied to the solar collector */
                var title = "Vælg beholderen som denne solfanger er forbundet til ";
                var message = "Hvis solfangeren ikke er forbundet til en beholder, tryk på \"Acceptér\"";
                var appliances = new ObservableCollection<Appliance>(AppliancesInPackagedSolution.Where
                                                 (a => a.DataSheet is ContainerDataSheet));
                RunSolarContainerDialog(message, title, appToAdd, appliances);
            }
            else
            {
                AddApplianceToPackagedSolution(appToAdd);
            }
        }

        private void UpdateEei()
        {
            PackagedSolution.Appliances = new ApplianceList(AppliancesInPackagedSolution.ToList());
            var results = _calculationManager.SelectCalculationStrategy(PackagedSolution);
            if (results != null && results.Count > 1)
            {
                EEIResultsRoomHeating = results[0].CalculateEEI(PackagedSolution).EEICharacters;
                EEIResultsWaterHeating = results[1].CalculateEEI(PackagedSolution).EEICharacters;
            }
            else
            {
                EEIResultsRoomHeating = results != null ? results[0].CalculateEEI(PackagedSolution).EEICharacters : "N/A";
                EEIResultsWaterHeating = "N/A";
            }
        }

        #endregion

    }
}
