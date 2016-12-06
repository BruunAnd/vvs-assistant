using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.Functions;
using System.Data.Entity;
using System.Threading.Tasks;
using VVSAssistant.Database;

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

        public AsyncObservableCollection<PackagedSolution> PackagedSolutions { get; set; }

        private readonly IDialogCoordinator _dialogCoordinator;

        public RelayCommand NavigateBackCmd { get; }
        public RelayCommand CreatePackagedSolutionCopyCmd { get; }
        public RelayCommand PrintCalculationCmd { get; }
        public RelayCommand DropPackagedSolutionCmd { get; }

        public ExistingPackagedSolutionsViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            PackagedSolutions = new AsyncObservableCollection<PackagedSolution>();
            SetupFilterableView(PackagedSolutions);

            NavigateBackCmd = new RelayCommand(x =>
            {
                NavigationService.GoBack();
            });

            CreatePackagedSolutionCopyCmd = new RelayCommand(async x =>
            {
                var createPackagedSolutionViewModel = new CreatePackagedSolutionViewModel(dialogCoordinator);
                await NavigationService.BeginNavigate(createPackagedSolutionViewModel);
                await Task.Run(() => createPackagedSolutionViewModel.LoadExistingPackagedSolution(SelectedPackagedSolution.Id));
                NavigationService.EndNavigate();
            }, x => SelectedPackagedSolution != null);

            DropPackagedSolutionCmd = new RelayCommand(x =>
            {
                DropPackagedSolution(SelectedPackagedSolution);
            }, x => SelectedPackagedSolution != null);
            
            PrintCalculationCmd = new RelayCommand(x =>
            {
                DataUtil.PdfEnergyLabel.ExportEnergyLabel(SelectedPackagedSolution);
            }, x => SelectedPackagedSolution != null);
        }

        private async void DropPackagedSolution(PackagedSolution packagedSolution)
        {
            using (var ctx = new AssistantContext())
            {
                var conflictingOffersList = ctx.Offers
                    .Where(o => o.PackagedSolution.Id == packagedSolution.Id)
                    .Include(o => o.OfferInformation)
                    .ToList();

                if (conflictingOffersList.Any())
                {
                    var formattedOffersString = string.Join("\n", conflictingOffersList.Select(x => $"- {x.OfferInformation.Title} ({x.CreationDate})"));
                    await _dialogCoordinator.ShowMessageAsync(this, "Fejl",
                        $"Pakkeløsningen kan ikke slettes, da den findes i følgende tilbud:\n{formattedOffersString}");
                    return;
                }
            }

            // No conflicts, remove the packaged solution
            PackagedSolutions.Remove(packagedSolution);

            using (var ctx = new AssistantContext())
            {
                ctx.PackagedSolutions.Attach(packagedSolution);
                ctx.PackagedSolutions.Remove(packagedSolution);
                ctx.SaveChanges();
            }
        }

        public override void LoadDataFromDatabase()
        {
            using (var ctx = new AssistantContext())
            {

                ctx.PackagedSolutions.Include(p => p.ApplianceInstances.Select(a => a.Appliance)).ToList().ForEach(PackagedSolutions.Add);
            }
        }

        protected override bool Filter(PackagedSolution obj)
        {
            return obj.Name.ContainsIgnoreCase(FilterString) || obj.Description.ContainsIgnoreCase(FilterString);
        }
    }
}
