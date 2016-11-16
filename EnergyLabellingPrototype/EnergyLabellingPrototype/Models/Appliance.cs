using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public class Appliance : UnitPrice, IFilterable
    {
        private static int count = 1;
        public readonly int Counter;
        
        public Appliance(string name, string description, string type, double price)
        {
            Name = name;
            Counter = count;
            Description = description;
            Type = type;
            Quantity = 1;
            UnitCostPrice = price;
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


        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged();
            }
        }
    }
}
