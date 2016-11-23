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
            set { SetProperty<int>(ref _quantity, value); OnPropertyChanged("CostPrice"); OnPropertyChanged("SalesPrice"); }
        }
        
        // Update CostPrice if the UnitCostPrice is changed.
        private double _unitCostPrice;
        public double UnitCostPrice
        {
            get { return _unitCostPrice; }
            set { SetProperty<double>(ref _unitCostPrice, value); OnPropertyChanged("CostPrice"); }
        }

        // Update SalesPrice if the UnitSalesPrice is changed.
        private double _unitSalesPrice;
        public double UnitSalesPrice
        {
            get { return _unitSalesPrice; }
            set { SetProperty<double>(ref _unitSalesPrice, value); OnPropertyChanged("SalesPrice"); }
        }
        

        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        public double CostPrice => UnitCostPrice * Quantity;
        public double ContributionMargin => SalesPrice - CostPrice;
    }
}
