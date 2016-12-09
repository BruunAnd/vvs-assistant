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

            DatabaseImport = new RelayCommand(x =>
            {
                var dlg = new OpenFileDialog {Filter = "Zip filer (.zip)|*.zip"};
                dlg.FileOk += ValidateDatabaseFile;
                var result = dlg.ShowDialog();
                if (result == true) DataUtil.Database.Import(dlg.FileName);
                DatabaseExport.NotifyCanExecuteChanged();
            });

            DatabaseExport = new RelayCommand(x =>
            {
                var dlg = new SaveFileDialog {Filter = "Zip filer (.zip)|*.zip", FileName = "database", DefaultExt = ".zip"};
                var result = dlg.ShowDialog();
                if (result == true) DataUtil.Database.Export(dlg.FileName);
            }, x => DataUtil.Database.Exists);

            RunOfferSettingsDialogCmd = new RelayCommand(x =>
            {
                OpenOfferSettingsDialog(instanceCanceled =>
                 {
                     _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                 }, instanceCompleted =>
                 {
                     _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                 });
            });

        }

        private void NavigationService_LoadingStateChanged(bool isLoading)
        {
            IsLoading = isLoading;
        }

        private static void ValidateDatabaseFile(object sender, CancelEventArgs e)
        {
            var dlg = sender as OpenFileDialog;

            if (dlg == null) return;
            using (var archive = ZipFile.OpenRead(dlg.FileName))
            {
                if (archive.Entries.FirstOrDefault(x => x.Name == DataUtil.Database.Name) != null) return;
                MessageBox.Show("Den valgte .zip fil indeholder ikke en gyldig database fil.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }
        
        public RelayCommand NavCommand { get; }
        public RelayCommand DatabaseImport { get; }
        public RelayCommand DatabaseExport { get; }
        public RelayCommand RunOfferSettingsDialogCmd { get; }

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
                            }, instanceCompleted =>
                            {
                                _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
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
                            }, instanceCompleted =>
                            {
                                _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                                
                            });
                        }
                    }
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
    }
}
