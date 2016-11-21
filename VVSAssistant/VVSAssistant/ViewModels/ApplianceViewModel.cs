using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.Interfaces;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class ApplianceViewModel : ViewModelBase, IFilterable
    {
        private Appliance _appliance;

        public ApplianceViewModel(Appliance appliance)
        {
            _appliance = appliance;
        }

        public string Name
        {
            get { return _appliance.Name; }
            set
            {
                if (_appliance.Name == value) return;
                _appliance.Name = value;
                OnPropertyChanged();
            }
        }

        public ApplianceTypes Type
        {
            get { return _appliance.Type; }
            set
            {
                if (_appliance.Type == value) return;
                _appliance.Type = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return "no description xd"; }
        }

        public bool DoesFilterMatch(string query)
        {
            return Name.ContainsIgnoreCase(query)
                   || Type.ToString().ContainsIgnoreCase(query)
                   || Description.ContainsIgnoreCase(query);
        }
    }
}
