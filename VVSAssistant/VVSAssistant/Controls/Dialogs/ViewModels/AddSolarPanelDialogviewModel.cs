using System;
using System.Collections.ObjectModel;
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
            //SolarPanelData.IsRoomHeater = false;
            //SolarPanelData.IsWaterHeater = false;
            SaveCommand = new RelayCommand(x =>
            {
                Save();
                completionHandler(this);
            }, x => IsWaterHeater || IsRoomHeater);

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
                SolarPanelData.IsWaterHeater = _isWaterHeater;
                SaveCommand.NotifyCanExecuteChanged();
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
                SolarPanelData.IsRoomHeater = _isRoomHeater;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
        private SolarCollectorDataSheet SolarPanelData => (_solarPanel?.DataSheet as SolarCollectorDataSheet);

        private void Save()
        {
            _appsInSolution.Add(_solarPanel);
        }
    }
}
