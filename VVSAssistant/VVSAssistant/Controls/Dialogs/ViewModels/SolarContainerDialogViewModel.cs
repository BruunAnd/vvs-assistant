using System;
using System.Collections.ObjectModel;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    internal class SolarContainerDialogViewModel : NotifyPropertyChanged
    {
        /* When a container is added to a packaged solution and there is a solar collector in it, 
         * this window should ask the user which container is tied to the solar collector. 
         * Furthermore, when a solar collector is added, and there are containers in the system, 
         * it should ask the user what container is connected to the solar collector. */

        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        private bool _solarContainer;
        private bool _nonSolarContainer;
        public bool SolarContainer
        {
            get { return _solarContainer; }
            set
            {
                _solarContainer = value;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
        public bool NonSolarContainer
        {
            get { return _nonSolarContainer; }
            set
            {
                _nonSolarContainer = value;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private ApplianceInstance _appliance;
        public ApplianceInstance Appliance
        {
            get { return _appliance; }
            set { _appliance = value; OnPropertyChanged(); }
        }

        private readonly ObservableCollection<ApplianceInstance> _appsInSolution;

        public SolarContainerDialogViewModel(ApplianceInstance appliance, 
            ObservableCollection<ApplianceInstance> appliances,
            Action<SolarContainerDialogViewModel> closeHandler, 
            Action<SolarContainerDialogViewModel> completionHandler)
        {
            Appliance = appliance;
            _appsInSolution = appliances;
            SaveCommand = new RelayCommand(x =>
            {
                HandleSaveCommand();
                completionHandler(this);
            }, x => SolarContainer || NonSolarContainer);

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }

        private void HandleSaveCommand()
        {
            Appliance.IsSolarContainer = SolarContainer;
            _appsInSolution.Add(Appliance);
        }
    }
}
