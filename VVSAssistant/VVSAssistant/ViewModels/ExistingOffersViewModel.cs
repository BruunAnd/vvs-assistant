using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Database;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;

namespace VVSAssistant.ViewModels
{
    public class ExistingOffersViewModel : ViewModelBase
    {
        public RelayCommand OpenOfferInCreateOfferViewModel { get; set; }

        private ObservableCollection<Offer> _offers;
        public ObservableCollection<Offer> Offers
        {
            get { return _offers; }
            set { _offers = value; OnPropertyChanged(); }
        }

        public Offer SelectedOffer { get; set; }

        public ExistingOffersViewModel(IDialogCoordinator coordinator)
        {
            OpenOfferInCreateOfferViewModel = new RelayCommand(async x =>
            {
                var createOfferViewModel = new CreateOfferViewModel(coordinator);
                await NavigationService.BeginNavigate(createOfferViewModel);
                await Task.Run(() => createOfferViewModel.LoadExistingOffer(SelectedOffer.Id));
                NavigationService.EndNavigate();
            }, x => SelectedOffer != null);
        }

        public override void LoadDataFromDatabase()
        {
            using (var ctx = new AssistantContext())
            {
                Offers = new ObservableCollection<Offer>(ctx.Offers
                    .Include(o => o.Materials)
                    .Include(o => o.Salaries)
                    .Include(o => o.PackagedSolution.ApplianceInstances.Select(a => a.Appliance))
                    .Include(o => o.OfferInformation)
                    .ToList());
            }
        }
    }
}
