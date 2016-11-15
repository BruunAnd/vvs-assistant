using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public static class Extensions
    {
        public static double TotalCostPrice<T>(this ObservableCollection<T> collection) where T : UnitPrice
        {
            return !collection.Any() ? 0 : collection.Select(x => x.CostPrice).Sum();
        }

        public static double TotalSalesPrice<T>(this ObservableCollection<T> collection) where T : UnitPrice
        {
            return !collection.Any() ? 0 : collection.Select(x => x.SalesPrice).Sum();
        }

        public static double TotalContributionMargin<T>(this ObservableCollection<T> collection) where T : UnitPrice
        {
            return collection.TotalSalesPrice() - collection.TotalCostPrice();
        }
    }
}
