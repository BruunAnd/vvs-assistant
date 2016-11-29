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
        private PackagedSolution _selectedPackagedSolution;
        public PackagedSolution SelectedPackagedSolution
        {
            get { return _selectedPackagedSolution; }
            set
            {
                SetProperty(ref _selectedPackagedSolution, value);
                CreatePackagedSolutionCopyCmd?.NotifyCanExecuteChanged();
                PrintCalculationCmd?.NotifyCanExecuteChanged();
                DropPackagedSolutionCmd?.NotifyCanExecuteChanged();
            }
        }

        public ObservableCollection<PackagedSolution> PackagedSolutions { get; } = new ObservableCollection<PackagedSolution>();
        private readonly IDialogCoordinator _dialogCoordinator;

        public RelayCommand CreatePackagedSolutionCopyCmd { get; }
        public RelayCommand PrintCalculationCmd { get; }
        public RelayCommand DropPackagedSolutionCmd { get; }

        public ExistingPackagedSolutionsViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            SetupFilterableView(PackagedSolutions);

            CreatePackagedSolutionCopyCmd = new RelayCommand(x =>
            {
                var createPackagedSolutionViewModel = new CreatePackagedSolutionViewModel(dialogCoordinator);
                NavigationService.NavigateTo(createPackagedSolutionViewModel);
                createPackagedSolutionViewModel.LoadExistingPackagedSolution(SelectedPackagedSolution.Id);
            }, x => SelectedPackagedSolution != null);

            DropPackagedSolutionCmd = new RelayCommand(x =>
            {
                DropPackagedSolution(SelectedPackagedSolution);
            }, x => SelectedPackagedSolution != null);
            
            PrintCalculationCmd = new RelayCommand(x =>
            {
                // TODO Make PackagedSolution aggregate calculation
                throw new NotImplementedException();
            });
        }

        private async void DropPackagedSolution(PackagedSolution packagedSolution)
        {
            var conflictingOffersList = DbContext.Offers.Where(o => o.PackagedSolution.Id == packagedSolution.Id).ToList();

            if (conflictingOffersList.Any())
            {
                var formattedOffersString = string.Join("\n", conflictingOffersList.Select(x => $"- {x.OfferInformation.Title} ({x.CreationDate})"));
                await _dialogCoordinator.ShowMessageAsync(this, "Fejl",
                    $"Pakkeløsningen kan ikke slettes, da den findes i følgende tilbud:\n{formattedOffersString}");
            }
            else
            {
                PackagedSolutions.Remove(packagedSolution);

                DbContext.PackagedSolutions.Remove(packagedSolution);
                DbContext.SaveChanges();
            }
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
