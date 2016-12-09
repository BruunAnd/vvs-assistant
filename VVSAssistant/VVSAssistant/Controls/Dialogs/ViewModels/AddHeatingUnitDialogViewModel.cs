using System;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddHeatingUnitDialogViewModel : NotifyPropertyChanged
    {
        public RelayCommand SaveCommand { get;  }
        public RelayCommand CloseCommand { get; }
        
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
        

        public AddHeatingUnitDialogViewModel(Action<AddHeatingUnitDialogViewModel> closeHandler, Action<AddHeatingUnitDialogViewModel> completionHandler)
        {
            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            }, x => IsSecondaryBoiler || IsPrimaryBoiler);


            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
