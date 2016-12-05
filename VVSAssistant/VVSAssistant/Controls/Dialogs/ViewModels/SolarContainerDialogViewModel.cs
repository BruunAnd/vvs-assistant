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

        public ObservableCollection<Appliance> Appliances { get; }

        private Appliance _selectedAppliance;
        public Appliance SelectedAppliance
        {
            get { return _selectedAppliance; }
            set { _selectedAppliance = value; OnPropertyChanged(); }
        }

        private Appliance _appliance;
        public Appliance Appliance
        {
            get { return _appliance; }
            set { _appliance = value; OnPropertyChanged(); }
        }

        private readonly ObservableCollection<Appliance> _appsInSolution;
        private readonly PackagedSolution _packagedSolution;

        public string Title { get; }
        public string Message { get; }

        public SolarContainerDialogViewModel(string message, string title, 
                                             Appliance appliance, ObservableCollection<Appliance> appliances, 
                                             ObservableCollection<Appliance> appsInSolution,PackagedSolution packagedSolution,
                                             Action<SolarContainerDialogViewModel> closeHandler, Action<SolarContainerDialogViewModel> completionHandler)
        {
            Title = title;
            Message = message;

            _appsInSolution = appsInSolution;
            _packagedSolution = packagedSolution;
            Appliances = appliances;
            Appliance = appliance;

            SaveCommand = new RelayCommand(x =>
            {
                HandleSaveCommand();
                completionHandler(this);
            });

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }

        private void HandleSaveCommand()
        {
            if (SelectedAppliance == null)
            {
                _appsInSolution.Add(Appliance); /* Type doesn't matter in this case */
            }

            else if (SelectedAppliance.DataSheet is ContainerDataSheet)
            {
                _packagedSolution.SolarContainers.Add(SelectedAppliance); /* Container, already in the PS */
                _appsInSolution.Add(Appliance); /* Solar Collector */
            }
            else if (SelectedAppliance.DataSheet is SolarCollectorDataSheet)
            {
                /* Don't need to do anything with the solar collector. */
                _packagedSolution.SolarContainers.Add(Appliance); /* Container */
                _appsInSolution.Add(Appliance); /* Container */
            }
        }

    }
}
