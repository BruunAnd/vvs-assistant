using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class SolarContainerDialogViewModel : NotifyPropertyChanged
    {
        /* When a container is added to a packaged solution and there is a solar collector in it, 
         * this window should ask the user which container is tied to the solar collector. 
         * Furthermore, when a solar collector is added, and there are containers in the system, 
         * it should ask the user what container is connected to the solar collector. */

        RelayCommand SaveCommand;
        RelayCommand CloseCommand;

        private ObservableCollection<Appliance> _appliances;
        public ObservableCollection<Appliance> Appliances
        {
            get { return _appliances; }
            set { _appliances = value; OnPropertyChanged(); }
        }

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

        private PackagedSolution _packagedSolution;

        public string Title { get; }
        public string Message { get; }

        public SolarContainerDialogViewModel(string title, string message, 
                                             Appliance appliance, ObservableCollection<Appliance> appliances, PackagedSolution packagedSolution,
                                             Action<SolarContainerDialogViewModel> closeHandler, Action<SolarContainerDialogViewModel> completionHandler)
        {
            Title = title;
            Message = message;

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
            if (SelectedAppliance.DataSheet is ContainerDataSheet)
            {
                _packagedSolution.SolarContainer = SelectedAppliance; /* Container */
                _packagedSolution.Appliances.Add(Appliance); /* Solar Collector */
            }
            else if (SelectedAppliance.DataSheet is SolarCollectorDataSheet)
            {
                _packagedSolution.SolarContainer = Appliance; /* Container */
                _packagedSolution.Appliances.Add(SelectedAppliance); /* Solar Collector */
            }
            else if (SelectedAppliance == null)
            {
                _packagedSolution.Appliances.Add(Appliance); /* Type doesn't matter in this case */
            }
        }

    }
}
