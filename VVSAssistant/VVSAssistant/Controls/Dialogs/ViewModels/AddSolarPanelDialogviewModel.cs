using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddSolarPanelDialogviewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }
        public AddSolarPanelDialogviewModel(Appliance solarPanel,
            ObservableCollection<Appliance> packagedsolution,
            Action<AddSolarPanelDialogviewModel> closeHandler, 
            Action<AddSolarPanelDialogviewModel> completionHandler)
        {
            _solarPanel = solarPanel;
            _appsInSolution = packagedsolution;
            SaveCommand = new RelayCommand(x =>
            {
                Save();
                completionHandler(this);
            });

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
        private readonly ObservableCollection<Appliance> _appsInSolution;
        private Appliance _solarPanel;
        private bool _isWaterHeater;
        private bool _isRoomHeater;
        public bool IsWaterHeater
        {
            get
            {
                return _isWaterHeater;
            }
            set
            {
                _isWaterHeater = value;
                SolarPanelData.isWaterHeater = _isWaterHeater;
            }
        }
        public bool IsRoomHeater
        {
            get
            {
                return _isRoomHeater;
            }
            set
            {
                _isRoomHeater = value;
                SolarPanelData.isRoomHeater = _isRoomHeater;
            }
        }
        private SolarCollectorDataSheet SolarPanelData
        {
            get
            {
                return (_solarPanel?.DataSheet as SolarCollectorDataSheet);
            }
        }
        private void Save()
        {
            _appsInSolution.Add(_solarPanel);
        }
    }
}
