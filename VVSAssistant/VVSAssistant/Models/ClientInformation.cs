using VVSAssistant.Common;

namespace VVSAssistant.Models
{
    public class ClientInformation : NotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set { SetProperty(ref _postalCode, value); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        
        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }
        
    }
}
