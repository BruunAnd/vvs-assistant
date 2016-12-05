
namespace VVSAssistant.Models
{
    public class Salary : UnitPrice
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
    }
}
