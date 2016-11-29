using System;
using System.Collections.ObjectModel;
using System.Linq;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Database;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Common;

namespace VVSAssistant.ViewModels
{
    public class ExistingOffersViewModel : ViewModelBase
    {
        public RelayCommand OpenOfferInCreateOfferViewModel;

        private ObservableCollection<Offer> _offers;
        public ObservableCollection<Offer> Offers
        {
            get { return _offers; }
            set { _offers = value; OnPropertyChanged(); }
        }

        public Offer SelectedOffer { get; set; }

        public ExistingOffersViewModel(IDialogCoordinator coordinator)
        {
            _offers = new ObservableCollection<Offer>();
            OpenOfferInCreateOfferViewModel = new RelayCommand(x =>
            {
                NavigationService.NavigateTo(new CreateOfferViewModel(coordinator, SelectedOffer));
            }, x => SelectedOffer != null
            );
        }

        public override void LoadDataFromDatabase()
        {
            DbContext.Offers.ToList().ForEach(o => Offers.Add(o));
        }
    }
}
