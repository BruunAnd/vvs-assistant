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
            set { SetProperty<string>(ref _name, value); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty<string>(ref _email, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { SetProperty<string>(ref _address, value); }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty<string>(ref _phoneNumber, value); }
        }
        
    }
}
