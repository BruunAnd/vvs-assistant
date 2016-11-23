using System;
using System.Collections.ObjectModel;
using System.Linq;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.Database;


namespace VVSAssistant.ViewModels
{
    public class ExistingOffersViewModel : ViewModelBase
    {
        private ObservableCollection<Offer> _offers;
        public ObservableCollection<Offer> Offers
        {
            get { return _offers; }
            set { _offers = value; OnPropertyChanged(); }
        }

        public ExistingOffersViewModel()
        {
            _offers = new ObservableCollection<Offer>();
        }

        public override void Initialize()
        {
            DbContext.Offers.ToList().ForEach(o => Offers.Add(o));
        }
    }
}
