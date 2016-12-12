using System;
using System.Collections.ObjectModel;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddSolarPanelDialogViewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        public AddSolarPanelDialogViewModel(ApplianceInstance solarPanel,
            ObservableCollection<ApplianceInstance> appliancesInSolution,
            Action<AddSolarPanelDialogViewModel> closeHandler, 
            Action<AddSolarPanelDialogViewModel> completionHandler)
        {
            _solarPanel = solarPanel;
            _appsInSolution = appliancesInSolution;
            _solarPanel.IsUsedForRoomHeating = false;
            _solarPanel.IsUsedForWaterHeating = false;

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

        private readonly ObservableCollection<ApplianceInstance> _appsInSolution;
        private readonly ApplianceInstance _solarPanel;

        public int Quantity { get; set; } = 1;

        private bool _isWaterHeater;
        public bool IsWaterHeater
        {
            get
            {
                return _isWaterHeater;
            }
            set
            {
                _isWaterHeater = value;
                _solarPanel.IsUsedForWaterHeating = _isWaterHeater;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isRoomHeater;
        public bool IsRoomHeater
        {
            get
            {
                return _isRoomHeater;
            }
            set
            {
                _isRoomHeater = value;
                _solarPanel.IsUsedForRoomHeating = _isRoomHeater;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private void Save()
        {
            while (Quantity != 0)
            {
                _appsInSolution.Add(_solarPanel);
                Quantity--;
            }
        }
    }
}
