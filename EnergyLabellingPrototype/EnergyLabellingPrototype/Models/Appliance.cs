using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public class Appliance : IFilterable, INotifyPropertyChanged
    {
        private static int count = 1;
        public readonly int Counter;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Appliance(string name, string description, string type, double price)
        {
            Name = name;
            Counter = count;
            Description = description;
            Type = type;
            Cost = price;
            count++;
        }

        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText) || Type.ToLower().Contains(filterText) || Description.ToLower().Contains(filterText);
        }
        
        public string Name { get; set; }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string type = null;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value != type)
                {
                    type = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private double cost;
        public double Cost
        {
            get
            {
                return cost;
            }
            set
            {
                if (value != cost)
                {
                    cost = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }

        private double salesPrice;
        public double SalesPrice
        {
            get
            {
                return salesPrice;
            }
            set
            {
                if (value != salesPrice)
                {
                    salesPrice = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }
        
        public double ContributionMargin { get { return SalesPrice - Cost; } }
    }
}
