using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    public partial class UnitPrice
    {
        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        public double CostPrice => UnitCostPrice * Quantity;
        public double ContributionMargin => SalesPrice - CostPrice;
    }
}
