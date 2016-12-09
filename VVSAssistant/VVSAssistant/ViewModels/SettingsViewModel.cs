using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {

        public RelayCommand DatabaseImport { get; }
        public RelayCommand DatabaseExport { get; }
        public RelayCommand RunOfferSettingsDialogCmd { get; }
        public RelayCommand NavigateBackCmd { get; }

        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly CustomDialog _customDialog = new CustomDialog();

        public SettingsViewModel(IDialogCoordinator coordinator)
        {
            _dialogCoordinator = coordinator;

            DatabaseImport = new RelayCommand(x =>
            {
                var dlg = new OpenFileDialog { Filter = "Zip filer (.zip)|*.zip" };
                dlg.FileOk += ValidateDatabaseFile;
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    DataUtil.Database.Import(dlg.FileName);
                    DisplayTimedMessage("Succes", "Databasen er importeret succesfuldt. ", 2);
                }
                DatabaseExport.NotifyCanExecuteChanged();
            });

            DatabaseExport = new RelayCommand(x =>
            {
                var dlg = new SaveFileDialog { Filter = "Zip filer (.zip)|*.zip", FileName = "database", DefaultExt = ".zip" };
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    DataUtil.Database.Export(dlg.FileName);
                    DisplayTimedMessage("Succes", "Databasen er eksporteret succesfuldt. ", 2);
                }
            }, x => DataUtil.Database.Exists);

            RunOfferSettingsDialogCmd = new RelayCommand(x =>
            {
                OpenOfferSettingsDialog(instanceCanceled =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                }, instanceCompleted =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, _customDialog);
                    DisplayTimedMessage("Information gemt!", "", 2);
                });
            });

            NavigateBackCmd = new RelayCommand(async x =>
            {
                if (!IsDataSaved)
                {
                    var result = await NavigationService.ConfirmDiscardChanges(_dialogCoordinator);
                    if (result == false) return;
                }
                NavigationService.GoBack();
            });
        }

        private async void OpenOfferSettingsDialog(Action<CompanyInfoDialogViewModel> closeHandler, Action<CompanyInfoDialogViewModel> completionHandler)
        {
            var dialogViewModel = new CompanyInfoDialogViewModel(closeHandler, completionHandler);
            dialogViewModel.LoadDataFromDatabase();
            _customDialog.Content = new CompanyInfoDialogView { DataContext = dialogViewModel };

            await _dialogCoordinator.ShowMetroDialogAsync(this, _customDialog);
        }

        private async void DisplayTimedMessage(string title, string message, double time)
        {
            var customDialog = new CustomDialog();
            var messageViewModel = new TimedMessageViewModel(title, message, time, instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));
            customDialog.Content = new TimesMessageView { DataContext = messageViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
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

        public override void LoadDataFromDatabase()
        {
            
        }
    }
}
