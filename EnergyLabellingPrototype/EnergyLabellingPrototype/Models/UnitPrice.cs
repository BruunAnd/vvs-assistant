using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype.Models
{
    public abstract class UnitPrice : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private int quantity;
        public int Quantity
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

        private double unitCostPrice;
        public double UnitCostPrice
        {
            get
            {
                return unitCostPrice;
            }
            set
            {
                if (value != unitCostPrice)
                {
                    unitCostPrice = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Cost");
                    NotifyPropertyChanged("ContributionMargin");
                }
            }
        }

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
        public double CostPrice { get { return UnitCostPrice * Quantity; } }
        public double ContributionMargin { get { return SalesPrice - CostPrice; } }
    }
}
