using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Database;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;
using VVSAssistant.Extensions;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    public class ExistingOffersViewModel : FilterableViewModelBase<Offer>
    {
        public RelayCommand NavigateBackCmd { get; }
        public RelayCommand OpenOfferInCreateOfferViewModel { get; }
        public RelayCommand PrintOfferCmd { get; }
        public RelayCommand DropOfferCmd { get; }

        private ObservableCollection<Offer> _offers;
        public ObservableCollection<Offer> Offers
        {
            get { return _offers; }
            set { _offers = value; OnPropertyChanged(); }
        }

        private Offer _selectedOffer;

        public Offer SelectedOffer
        {
            get { return _selectedOffer; }
            set
            {
                SetProperty(ref _selectedOffer, value);
                DropOfferCmd.NotifyCanExecuteChanged();
                PrintOfferCmd.NotifyCanExecuteChanged();
                OpenOfferInCreateOfferViewModel.NotifyCanExecuteChanged();
            }
        }

        public ExistingOffersViewModel(IDialogCoordinator coordinator)
        {
            NavigateBackCmd = new RelayCommand(x =>
            {
                NavigationService.GoBack();
            });

            OpenOfferInCreateOfferViewModel = new RelayCommand(async x =>
            {
                var createOfferViewModel = new CreateOfferViewModel(coordinator);
                await NavigationService.BeginNavigate(createOfferViewModel);
                await Task.Run(() => createOfferViewModel.LoadExistingOffer(SelectedOffer.Id));
                NavigationService.EndNavigate();
            }, x => SelectedOffer != null);

            PrintOfferCmd = new RelayCommand(x =>
            {
                DataUtil.Offer.Export(SelectedOffer);
            }, x => SelectedOffer != null);

            DropOfferCmd = new RelayCommand(x =>
            {
                DropExistingOffer(SelectedOffer);
            }, x => SelectedOffer != null);
        }

        private void DropExistingOffer(Offer offer)
        {
            // Remove offer from local observable collection
            Offers.Remove(offer);

            // Open a database context and drop offer
            using (var ctx = new AssistantContext())
            {
                ctx.Offers.Attach(offer);
                ctx.Offers.Remove(offer);
                ctx.SaveChanges();
            }
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
                    .Include(o => o.Client.ClientInformation)
                    .ToList());
            }
            SetupFilterableView(Offers);
        }

        protected override bool Filter(Offer obj)
        {
            return obj.OfferInformation.Title.ContainsIgnoreCase(FilterString);
        }
    }
}
