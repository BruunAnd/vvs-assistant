using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddHeatingUnitDialogViewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get;  }
        public RelayCommand CloseCommand { get; }

        public string Title { get; }
        public string Message { get; }

        private bool _isPrimaryBoiler;
        public bool IsPrimaryBoiler
        {
            get { return _isPrimaryBoiler; }
            set
            {
                SetProperty(ref _isPrimaryBoiler, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isSecondaryBoiler;
        public bool IsSecondaryBoiler
        {
            get { return _isSecondaryBoiler; }
            set
            {
                SetProperty(ref _isSecondaryBoiler, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isUsedForRoomHeating;
        public bool IsUsedForRoomHeating
        {
            get { return _isUsedForRoomHeating; }
            set
            {
                SetProperty(ref _isUsedForRoomHeating, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isUsedForWaterHeating;
        public bool IsUsedForWaterHeating
        {
            get { return _isUsedForWaterHeating; }
            set
            {
                SetProperty(ref _isUsedForWaterHeating, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }


        public AddHeatingUnitDialogViewModel(Action<AddHeatingUnitDialogViewModel> closeHandler, Action<AddHeatingUnitDialogViewModel> completionHandler)
        {
            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            }, x => IsSecondaryBoiler || (IsPrimaryBoiler && (IsUsedForRoomHeating || IsUsedForWaterHeating)));


            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
