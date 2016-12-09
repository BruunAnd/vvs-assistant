using System;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Functions;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Database;

namespace VVSAssistant.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            NavigationService.LoadingStateChanged += NavigationService_LoadingStateChanged;

            NavCommand = new RelayCommand(x =>
            {
                var str = x as string;
                OnNav(str);
            });

        }

        private void NavigationService_LoadingStateChanged(bool isLoading)
        {
            IsLoading = isLoading;
        }
        
        public RelayCommand NavCommand { get; }

        private readonly DialogCoordinator _dialogCoordinator = new DialogCoordinator();
        private readonly CustomDialog _customDialog = new CustomDialog();

        private async void OnNav(string destination)
        {
            ViewModelBase nextPage;

            switch (destination)
            {
                case "ExistingPackagedSolutionView":
                    nextPage = new ExistingPackagedSolutionsViewModel(_dialogCoordinator);
                    break;
                case "CreatePackagedSolutionView":
                    using (var ctx = new AssistantContext())
                    {
                        nextPage = new CreatePackagedSolutionViewModel(_dialogCoordinator);
                        if (!ctx.CompanyInformation.Any())
                        {
                            OpenOfferSettingsDialog(instanceCanceled =>
                            {
                                _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                                NavigationService.GoBack();
                            }, async instanceCompleted =>
                            {
                                await _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                                DisplayTimedMessage("Information gemt!", "", 2);
                            });
                        }
                    }
                    
                    break;
                case "ExistingOffersView":
                    nextPage = new ExistingOffersViewModel(_dialogCoordinator);
                    break;
                case "CreateOfferView":
                    using (var ctx = new AssistantContext())
                    {
                        nextPage = new CreateOfferViewModel(_dialogCoordinator);
                        if (!ctx.CompanyInformation.Any())
                        {
                            OpenOfferSettingsDialog(instanceCanceled =>
                            {
                                _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                                NavigationService.GoBack();
                            }, async instanceCompleted =>
                            {
                                await _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                                DisplayTimedMessage("Information gemt!", "", 2);
                            });
                        }
                    }
                    break;
                case "SettingsView":
                    nextPage = new SettingsViewModel(_dialogCoordinator);
                    break;
                case "GoBack":
                    NavigationService.GoBack();
                    return;
                default:
                    return; 
            }

            // Navigate to page
            await NavigationService.BeginNavigate(nextPage);
            NavigationService.EndNavigate();
        }

        public async void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Cancel close command
            e.Cancel = true;
            var result = true;

            // If current page is not null, run close window confirmation dialog
            if(NavigationService.CurrentPage != null && NavigationService.CurrentPage.IsDataSaved == false) result = 
                    await NavigationService.ConfirmDiscardChanges(new DialogCoordinator());

            // Don't close the window
            if (!result) return;

            // Close the window
            Application.Current.Shutdown();

        }

        private async void OpenOfferSettingsDialog(Action<CompanyInfoDialogViewModel> closeHandler, Action<CompanyInfoDialogViewModel> completionHandler)
        { 
            IsLoading = true;
            var dialogViewModel = new CompanyInfoDialogViewModel(closeHandler, completionHandler);
            dialogViewModel.LoadDataFromDatabase();
            IsLoading = false;
            _customDialog.Content = new CompanyInfoDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, _customDialog);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private async void DisplayTimedMessage(string title, string message, double time)
        {
            var customDialog = new CustomDialog();
            var messageViewModel = new TimedMessageViewModel(title, message, time, instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));
            customDialog.Content = new TimesMessageView { DataContext = messageViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }
    }
}
