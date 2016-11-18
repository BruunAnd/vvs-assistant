using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels
{
    class ApplianceViewModel : ObservableObject
    {

        public ApplianceViewModel()
        {

        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type == value) return;
                _Type = value;
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value) return;
                _Description = value;
                OnPropertyChanged();
            }
        }

        public void Push()
        {
            // Should call a method in the model to push the data in this object to the model / database.
            throw new NotImplementedException();
        }

        public void Pull()
        {
            // Should call a method in the model to pull data from the model / database to override this object.
            throw new NotImplementedException();
        }

    }
}
