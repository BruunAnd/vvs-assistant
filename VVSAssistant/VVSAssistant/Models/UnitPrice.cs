using System;
using VVSAssistant.Common;

namespace VVSAssistant.Models
{
    public class UnitPrice : NotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); OnPropertyChanged("CostPrice"); OnPropertyChanged("SalesPrice"); OnPropertyChanged("ContributionMargin"); }
        }
        
        // Update CostPrice if the UnitCostPrice is changed.
        private double _unitCostPrice;
        public double UnitCostPrice
        {
            get { return _unitCostPrice; }
            set { SetProperty(ref _unitCostPrice, value); OnPropertyChanged("CostPrice"); OnPropertyChanged("ContributionMargin"); }
        }

        // Update SalesPrice if the UnitSalesPrice is changed.
        private double _unitSalesPrice;
        public double UnitSalesPrice
        {
            get { return _unitSalesPrice; }
            set { SetProperty(ref _unitSalesPrice, value); OnPropertyChanged("SalesPrice"); OnPropertyChanged("ContributionMargin"); }
        }

        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        public double CostPrice => UnitCostPrice * Quantity;
        public double ContributionMargin => SalesPrice - CostPrice;
        
    }
}
