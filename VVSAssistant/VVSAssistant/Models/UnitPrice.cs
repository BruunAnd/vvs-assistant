using System;
using VVSAssistant.Common;
using VVSAssistant.ValueConverters;

namespace VVSAssistant.Models
{
    public class UnitPrice : NotifyPropertyChanged
    {
        /* EF6 requires an empty constructor, since we have a constructor for appliances */
        public UnitPrice() {}

        public UnitPrice(Appliance appliance)
        {
            Name = appliance.Name;
            Description = ApplianceTypeConverter.ConvertTypeToString(appliance.Type);
            Quantity = 1;
            UnitCostPrice = appliance.DataSheet.Price;
        }

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


        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        public double CostPrice => UnitCostPrice * Quantity;
        public double ContributionMargin => SalesPrice - CostPrice;
        
    }
}
