using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype
{
    public class Material : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double number;
        public double Number
        {
            get
            {
                return number;
            }
            set
            {
                if (value != number)
                {
                    number = value;
                    NotifyPropertyChanged();
                }
            }
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
                }
            }
        }

        public double SalesPrice { get { return Math.Abs(UnitSalesPrice * Quantity); } }

        public double ContributionMargin { get { return SalesPrice - Cost; } }
    }
}
