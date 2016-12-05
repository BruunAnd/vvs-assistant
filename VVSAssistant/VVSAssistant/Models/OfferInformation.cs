using VVSAssistant.Common;

namespace VVSAssistant.Models
{
    public class OfferInformation : NotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _intro;
        public string Intro
        {
            get { return _intro; }
            set { SetProperty(ref _intro, value); }
        }

        private string _signature;
        public string Signature
        {
            get { return _signature; }
            set { SetProperty(ref _signature, value); }
        }


        private string _outro;
        public string Outro
        {
            get { return _outro; }
            set { SetProperty(ref _outro, value); }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private bool _applyTax;
        public bool ApplyTax
        {
            get { return _applyTax; }
            set { SetProperty(ref _applyTax, value); }
        }
    }
}
