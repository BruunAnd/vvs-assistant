using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    public class UnitPrice
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double UnitCostPrice { get; set; }
        public double UnitSalesPrice { get; set; }

        [NotMapped]
        public double SalesPrice => Math.Abs(UnitSalesPrice * Quantity);
        [NotMapped]
        public double CostPrice => UnitCostPrice * Quantity;
        [NotMapped]
        public double ContributionMargin => SalesPrice - CostPrice;
    }
}
