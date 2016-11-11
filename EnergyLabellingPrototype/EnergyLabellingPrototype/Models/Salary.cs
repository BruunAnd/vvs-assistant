using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype.Models
{
    public class Salary : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string Name { get; set; }

        private double quantity;
        public double Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value != quantity)
                {
                    quantity = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Cost");
                    NotifyPropertyChanged("SalesPrice");
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }

        private double unitCost;
        public double UnitCost
        {
            get
            {
                return unitCost;
            }
            set
            {
                if (value != unitCost)
                {
                    unitCost = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Cost");
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }

        public double Cost { get { return UnitCost * Quantity; } }

        private double unitSalesPrice;
        public double UnitSalesPrice
        {
            get
            {
                return unitSalesPrice;
            }
            set
            {
                if (value != unitSalesPrice)
                {
                    unitSalesPrice = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("SalesPrice");
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }

        public double SalesPrice { get { return Math.Abs(UnitSalesPrice * Quantity); } }

        public double ContributionMargin { get { return SalesPrice - Cost; } }
    }
}
