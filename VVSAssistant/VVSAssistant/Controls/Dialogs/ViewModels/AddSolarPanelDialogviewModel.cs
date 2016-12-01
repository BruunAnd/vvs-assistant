using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddSolarPanelDialogviewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }
        public AddSolarPanelDialogviewModel()
        {

        }
        private bool _isWaterHeater;
        private bool _isRoomHeater;
        public bool IsWaterHeater
        {
            get
            {
                return false;
            }
            set
            {
                SetProperty(ref _isWaterHeater, value);
                
            }
        }
        public bool IsRoomHeater
        {
            get
            {
                return false;
            }
            set
            {
                SetProperty(ref _isRoomHeater, value);
            }
        }
    }
}
