using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class SaveDialogViewModel : ViewModelBase
    {
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }

        public string Title { get; }

        public string Message { get; }

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                SetProperty(ref _input, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public SaveDialogViewModel(string title, string message, Action<SaveDialogViewModel> closeHandler, Action<SaveDialogViewModel> completionHandler)
        {
            Title = title;
            Message = message;

            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            }, x => !string.IsNullOrEmpty(Input));


            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
