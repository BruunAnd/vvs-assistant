using System.IO.Compression;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    internal class MainWindowViewModel : NotifyPropertyChanged
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
                if (result == true) DatabasePortation.Import(dlg.FileName);
                DatabaseExport.NotifyCanExecuteChanged();
            });

            DatabaseExport = new RelayCommand(x =>
            {
                var dlg = new SaveFileDialog {Filter = "Zip filer (.zip)|*.zip", FileName = "database", DefaultExt = ".zip"};
                var result = dlg.ShowDialog();
                if (result == true) DatabasePortation.Export(dlg.FileName);
            }, x => DatabasePortation.Exists());
        }

        private void ValidateDatabaseFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlg = sender as OpenFileDialog;

            using (var archive = ZipFile.OpenRead(dlg.FileName))
            {
                if (archive.Entries.FirstOrDefault(x => x.Name == DatabasePortation.Name()) != null) return;
                MessageBox.Show("Den valgte .zip fil indeholder ikke en gyldig database fil.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                SetProperty(ref _currentViewModel, value);
            }
        }
        
        public RelayCommand NavCommand { get; }
        public RelayCommand DatabaseImport { get; }
        public RelayCommand DatabaseExport { get; }
        public RelayCommand ImportVVSCatalogue { get; }
        public RelayCommand ImportSalesCatalogue { get; }
        



        private void OnNav(string destination)
        {
            CurrentViewModel?.CloseDataConnection();

            switch (destination)
            {
                case "ExistingPackagedSolutionView":
                    CurrentViewModel = new ExistingPackagedSolutionsViewModel();
                    break;
                case "CreatePackagedSolutionView":
                    CurrentViewModel = new CreatePackagedSolutionViewModel(new DialogCoordinator());
                    break;
                case "ExistingOffersView":
                    CurrentViewModel = new ExistingOffersViewModel();
                    break;
                case "CreateOfferView":
                    CurrentViewModel = new CreateOfferViewModel(new DialogCoordinator());
                    break;
                default:
                    CurrentViewModel = null;
                    return; // Don't open dataconnection
            }

            CurrentViewModel?.OpenDataConnection();
            CurrentViewModel?.LoadDataFromDatabase();
        }
    }
}
