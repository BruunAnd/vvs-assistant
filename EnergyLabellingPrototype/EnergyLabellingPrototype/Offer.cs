using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype
{
    public class Offer : IFilterable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText);
        }

        public Offer()
        {
            Date = DateTime.Now.ToString();
            Counter = count;
            count++;
        }
        
        public string Name { get; set; }
        
        private Solution solution;
        public Solution Solution
        {
            get
            {
                return solution;
            }
            set
            {
                if (value != solution)
                {
                    solution = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double ComponentCost { get { return Solution.Components.Select(x => x.Cost).Sum(); } }
        public double ComponentSalesPrice { get { return Solution.Components.Select(x => x.SalesPrice).Sum(); } }
        public double ComponentContributionMargin { get { return ComponentSalesPrice - ComponentCost; } }

        public ObservableCollection<Salary> SalaryList = new ObservableCollection<Salary>();
        public double SalaryCost { get { return SalaryList.Select(x => x.Cost).Sum(); } }
        public double SalarySalesPrice { get { return SalaryList.Select(x => x.SalesPrice).Sum(); } }
        public double SalaryContributionMargin { get { return SalarySalesPrice - SalaryCost; } }

        public ObservableCollection<Material> MaterialList = new ObservableCollection<Material>();
        public double MaterialCost { get { return MaterialList.Select(x => x.Cost).Sum(); } }
        public double MaterialSalesPrice { get { return MaterialList.Select(x => x.SalesPrice).Sum(); } }
        public double MaterialContributionMargin { get { return MaterialSalesPrice - MaterialCost; } }

        public double TotalCost { get { return MaterialCost + SalaryCost + ComponentCost; } }
        public double TotalSalesPrice { get { return MaterialSalesPrice + SalarySalesPrice + ComponentSalesPrice; } }
        public double TotalContributionMargin { get { return MaterialContributionMargin + SalaryContributionMargin + ComponentContributionMargin; } }
        public double TotalSalesPricePlusTax { get { return TotalSalesPrice * 1.25; } }

        public string Date { get; set; }

        private static int count = 1;
        public readonly int Counter;

    }
}
