using System;
using System.Collections.ObjectModel;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.Extensions;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    internal class ExistingPackagedSolutionsViewModel : FilterableViewModelBase<PackagedSolution>
    {
        public ObservableCollection<PackagedSolution> PackagedSolutions { get; } = new ObservableCollection<PackagedSolution>();
        private readonly IDialogCoordinator _dialogCoordinator;
        public PackagedSolution SelectedPackagedSolution { get; set; }

        public RelayCommand CreatePackagedSolutionCopyCmd { get; }
        public RelayCommand PrintCalculationCmd { get; }
        public RelayCommand DropPackagedSolutionCmd { get; }

        public ExistingPackagedSolutionsViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            SetupFilterableView(PackagedSolutions);

            CreatePackagedSolutionCopyCmd = new RelayCommand(x => NavigationService.NavigateTo(new CreatePackagedSolutionViewModel(dialogCoordinator, SelectedPackagedSolution)));
        }

        public override void LoadDataFromDatabase()
        {
            DbContext.PackagedSolutions.ToList().ForEach(PackagedSolutions.Add);
        }

        protected override bool Filter(PackagedSolution obj)
        {
            return obj.Name.ContainsIgnoreCase(FilterString) || obj.Description.ContainsIgnoreCase(FilterString);
        }
    }
}
