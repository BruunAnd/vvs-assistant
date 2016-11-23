using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Collections.Specialized;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Extensions;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    public class CreatePackagedSolutionViewModel : FilterableViewModelBase<Appliance>
    {
        #region Property initializations
        public PackagedSolution PackagedSolution { get; } = new PackagedSolution();
        private IDialogCoordinator _dialogCoordinator;
        public Appliance SelectedAppliance { get; set; }
        #endregion

        #region Command initializations
        public RelayCommand AddApplianceToPackageSolution { get; }
        public RelayCommand RemoveApplianceFromPackageSolution { get; }
        public RelayCommand EditAppliance { get; }
        public RelayCommand RemoveAppliance { get; }
        public RelayCommand NewPackageSolution { get; }
        public RelayCommand SaveDialog { get; }
        #endregion

        #region Collections
        public ObservableCollection<Appliance> Appliances { get; } = new ObservableCollection<Appliance>();
        #endregion

        public CreatePackagedSolutionViewModel(IDialogCoordinator dialogCoordinator)
        {
            SetupFilterableView(Appliances);
            _dialogCoordinator = dialogCoordinator;

            // Load list of appliances from database
            foreach (var appliance in new AssistantContext().Appliances)
            {
                Appliances.Add(appliance);
            }

            #region Command declarations

            AddApplianceToPackageSolution = new RelayCommand(x => 
            {
                PackagedSolution.Appliances.Add(SelectedAppliance);
                RaiseCanExecuteChanged();
            }, x => SelectedAppliance != null);

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                PackagedSolution.Appliances.Remove(SelectedAppliance);
                RaiseCanExecuteChanged();
            }, x => SelectedAppliance != null);

            EditAppliance = new RelayCommand(x =>
             {
                 RunEditDialog();
             });

            RemoveAppliance = new RelayCommand(x =>
            {
                // SelectedAppliance.RemoveFromDatabase();
                
                Appliances.Remove(SelectedAppliance);
            }, x => SelectedAppliance != null);

            NewPackageSolution = new RelayCommand(x =>
            {
                PackagedSolution.Appliances.Clear();
                RaiseCanExecuteChanged();
            }, x => PackagedSolution.Appliances.Any());
            

            SaveDialog = new RelayCommand(x =>
            {
                RunSaveDialog();
            }, x => PackagedSolution.Appliances.Any());
            #endregion
        }

        private void RaiseCanExecuteChanged()
        {
            NewPackageSolution.NotifyCanExecuteChanged();
            SaveDialog.NotifyCanExecuteChanged();
        }

        private async void RunSaveDialog()
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new SaveDialogViewModel("Gem pakkeløsning", "Navn:",
                instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog),
                instanceCompleted =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    PackagedSolution.Name = instanceCompleted.Input;
                    // PackagedSolution.SaveToDatabase();
                }); 
            
            customDialog.Content = new SaveDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async void RunEditDialog()
        {
            var customDialog = new CustomDialog();

            var dialogViewModel = new EditApplianceViewModel(SelectedAppliance, instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog), instanceCompleted => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));

            customDialog.Content = new EditApplianceView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }


        protected override bool Filter(Appliance filterObj)
        {
            return filterObj.Name.ContainsIgnoreCase(FilterString) ||
                   filterObj.Type.ToString().ContainsIgnoreCase(FilterString);
        }
    }
}
