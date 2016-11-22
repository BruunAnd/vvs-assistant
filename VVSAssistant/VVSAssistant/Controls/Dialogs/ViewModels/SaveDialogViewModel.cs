using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    public class SaveDialogViewModel : ViewModelBase
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public delegate void DialogFilledHandler(string input);
        public DialogFilledHandler DialogFilled;

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

        public SaveDialogViewModel(string title, string message, IDialogCoordinator dialogCoordinator, Action<SaveDialogViewModel> closeHandler)
        {
            _dialogCoordinator = dialogCoordinator;

            Title = title;
            Message = message;

            SaveCommand = new RelayCommand(x =>
            {
                ConfirmSaveDialog(closeHandler);
            }, x => !string.IsNullOrEmpty(Input));
            

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }

        private void ConfirmSaveDialog(Action<SaveDialogViewModel> closeHandler)
        {
            closeHandler(this);
            DialogFilled?.Invoke(Input);
        }
    }
}
