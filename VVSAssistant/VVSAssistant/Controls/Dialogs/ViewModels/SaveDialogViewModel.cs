using System;
using VVSAssistant.ViewModels.MVVM;

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
                SetProperty<string>(ref _input, value);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public SaveDialogViewModel(string title, string message, Action<SaveDialogViewModel> closeHandler)
        {
            Title = title;
            Message = message;

            SaveCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            }, x => !string.IsNullOrEmpty(Input));
            

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
