using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
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
            }, x => DataUtil.Database.Exists());
            
        }
        

        private void ValidateDatabaseFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = sender as OpenFileDialog;

            using (var archive = ZipFile.OpenRead(dlg.FileName))
            {
                if (archive.Entries.FirstOrDefault(x => x.Name == DataUtil.Database.Name()) != null) return;
                MessageBox.Show("Den valgte .zip fil indeholder ikke en gyldig database fil.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }

        //private static ViewModelBase _currentViewModel;
        //public static ViewModelBase CurrentViewModel
        //{
        //    get { return _currentViewModel; }
        //    set
        //    {
        //        if (value == null) return;
        //        _currentViewModel = value;
        //        OnStaticPropertyChanged("CurrentViewModel");
        //    }
        //}
        
        public RelayCommand NavCommand { get; }
        public RelayCommand DatabaseImport { get; }
        public RelayCommand DatabaseExport { get; }
        
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "ExistingPackagedSolutionView":
                    NavigationService.NavigateTo(new ExistingPackagedSolutionsViewModel(new DialogCoordinator()));
                    break;
                case "CreatePackagedSolutionView":
                    NavigationService.NavigateTo(new CreatePackagedSolutionViewModel(new DialogCoordinator()));
                    break;
                case "ExistingOffersView":
                    NavigationService.NavigateTo(new ExistingOffersViewModel());
                    break;
                case "CreateOfferView":
                    NavigationService.NavigateTo(new CreateOfferViewModel(new DialogCoordinator()));
                    break;
                case "GoBack":
                    NavigationService.GoBack();
                    break;
                default:
                    return; 
            }
        }
    }
}
