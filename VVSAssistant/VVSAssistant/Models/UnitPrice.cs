using System;

namespace VVSAssistant.Models
{
    public class UnitPrice
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double UnitCostPrice { get; set; }
        public double UnitSalesPrice { get; set; }

        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        public double CostPrice => UnitCostPrice * Quantity;
        public double ContributionMargin => SalesPrice - CostPrice;
    }
}
