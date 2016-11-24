using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class AddBoilerDialogViewModel : NotifyPropertyChanged
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

        public AddBoilerDialogViewModel(string title, string message, Action<AddBoilerDialogViewModel> closeHandler, Action<AddBoilerDialogViewModel> completionHandler)
        {
            Title = title;
            Message = message;

            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            }, x => IsPrimaryBoiler || IsSecondaryBoiler);


            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
