//using System;
//using System.Threading;
//using VVSAssistant.ViewModels.MVVM;
//using MahApps.Metro.Controls.Dialogs;

//namespace VVSAssistant.Controls.Dialogs.ViewModels
//{
//    public class GenerateOfferDialogViewModel : ViewModelBase
//    {
//        private IDialogCoordinator _dialogCoordinator;

//        public RelayCommand CloseCommand { get; }
//        public RelayCommand NextCommand { get; }
        
//        private string _title;
//        public string Title
//        {
//            get { return _title; }
//            set
//            {
//                SetProperty<string>(ref _title, value);
//                // SaveCommand.NotifyCanExecuteChanged();
//            }
//        }

//        private string _intro;
//        public string Intro
//        {
//            get { return _intro; }
//            set
//            {
//                SetProperty<string>(ref _intro, value);
//            }
//        }

//        private string _outro;

//        public string Outro
//        {
//            get { return _outro; }
//            set { SetProperty<string>(ref _outro, value); }
//        }

//        private bool _applyTax;

//        public bool ApplyTax
//        {
//            get { return _applyTax; }
//            set { SetProperty<bool>(ref _applyTax, value); }
//        }


//        public GenerateOfferDialogViewModel(IDialogCoordinator dialogCoordinator, Action<SaveDialogViewModel> closeHandler)
//        {
//            _dialogCoordinator = dialogCoordinator;
            
//            NextCommand = new RelayCommand(x =>
//            {
//                ConfirmSaveDialog(closeHandler);
//            }, x => !string.IsNullOrEmpty(Input));


//            CloseCommand = new RelayCommand(x =>
//            {
//                closeHandler(this);
//            });
//        }

//        private async void ConfirmSaveDialog(Action<SaveDialogViewModel> closeHandler)
//        {
//            await _dialogCoordinator.ShowMessageAsync(this, "Gemt", "Din pakkeløsning blev gemt under navnet " + Input);
//            closeHandler(this);
//        }
//    }
//}

