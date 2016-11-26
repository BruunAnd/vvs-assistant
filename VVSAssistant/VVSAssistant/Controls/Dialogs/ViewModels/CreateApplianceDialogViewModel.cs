using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class CreateApplianceDialogViewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        private string _chosenType;
        public string ChosenType
        {
            get { return _chosenType; }
            set { _chosenType = value; OnTypeChosen(); }
        }

        public ObservableCollection<string> Types { get; } = new ObservableCollection<string>()
        { "Varmepumpe", "Lavtemperatursvarmepumpe", "Kraftvarmeanlæg", "Kedel", "Beholder", "Solpanel", "Solstation", "Temperaturregulering"};

        private Appliance _newAppliance;
        public Appliance NewAppliance
        {
            get { return _newAppliance; }
            set { _newAppliance = value; OnPropertyChanged(); }
        }

        private bool isHeatingOrSolar = false;
        public bool IsHeatingOrSolar
        {
            get { return isHeatingOrSolar; }
            set { isHeatingOrSolar = value; OnPropertyChanged(); }
        }

        private bool isContainer = false;
        public bool IsContainer
        {
            get { return isContainer; }
            set { isContainer = value; OnPropertyChanged(); }
        }

        public bool IsWaterContainer
        {
            get
            {
                if (NewAppliance.DataSheet != null)
                    return (NewAppliance.DataSheet as ContainerDataSheet).isWaterContainer;
                else
                    return false;
            }
            set
            {
                if (NewAppliance.DataSheet != null)
                {
                    (NewAppliance.DataSheet as ContainerDataSheet).isWaterContainer = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnTypeChosen()
        {
            switch (ChosenType)
            {
                case "Varmepumpe":
                    _newAppliance.Type = ApplianceTypes.HeatPump;
                    _newAppliance.DataSheet = new HeatingUnitDataSheet();
                    IsHeatingOrSolar = true;
                    break;
                case "Lavtemperatursvarmepumpe":
                    _newAppliance.Type = ApplianceTypes.LowTempHeatPump;
                    _newAppliance.DataSheet = new HeatingUnitDataSheet();
                    IsHeatingOrSolar = true;
                    break;
                case "Kraftvarmeanlæg":
                    _newAppliance.Type = ApplianceTypes.CHP;
                    _newAppliance.DataSheet = new HeatingUnitDataSheet();
                    IsHeatingOrSolar = true;
                    break;
                case "Kedel":
                    _newAppliance.Type = ApplianceTypes.Boiler;
                    _newAppliance.DataSheet = new HeatingUnitDataSheet();
                    IsHeatingOrSolar = true;
                    break;
                case "Beholder":
                    _newAppliance.Type = ApplianceTypes.Container;
                    _newAppliance.DataSheet = new ContainerDataSheet();
                    IsContainer = true;
                    break;
                case "Solpanel":
                    _newAppliance.Type = ApplianceTypes.SolarPanel;
                    _newAppliance.DataSheet = new SolarCollectorDataSheet();
                    IsHeatingOrSolar = true;
                    break;
                case "Solstation":
                    _newAppliance.Type = ApplianceTypes.SolarStation;
                    _newAppliance.DataSheet = new SolarStationDataSheet();
                    break;
                case "Temperaturregulering":
                    _newAppliance.Type = ApplianceTypes.TemperatureController;
                    _newAppliance.DataSheet = new TemperatureControllerDataSheet();
                    break;
                default:
                    // No type recognised...
                    break;
            }
            OnPropertyChanged("NewAppliance");
        }

        public CreateApplianceDialogViewModel(Appliance newAppliance, Action<CreateApplianceDialogViewModel> closeHandler, Action<CreateApplianceDialogViewModel> completionHandler)
        {
            NewAppliance = newAppliance;
            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });

            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            });
        }

        private void HasAnyNullProperties(object obj)
        {

        }
    }
}
