using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public class Offer : IFilterable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public Offer()
        {
            Date = DateTime.Now.ToString();
            Counter = count;
            count++;
            
            Materials.CollectionChanged += MaterialsCollectionChanged;
            Salaries.CollectionChanged += SalariesCollectionChanged;
        }
        

        private void MaterialsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Material item in e.NewItems)
                {
                    item.PropertyChanged += AnyPropertyChanged;
                }

            }

            if (e.OldItems != null)
            {
                foreach (Material item in e.OldItems)
                {
                    item.PropertyChanged -= AnyPropertyChanged;
                }
            }
            
            AnyPropertyChanged(sender, null);
        }

        private void SalariesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Salary item in e.NewItems)
                {
                    item.PropertyChanged += AnyPropertyChanged;
                }

            }

            if (e.OldItems != null)
            {
                foreach (Salary item in e.OldItems)
                {
                    item.PropertyChanged -= AnyPropertyChanged;
                }
            }
            
            AnyPropertyChanged(sender, null);
        }

        private void AnyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("ApplianceCost");
            NotifyPropertyChanged("ApplianceSalesPrice");
            NotifyPropertyChanged("ApplianceContributionMargin");
            NotifyPropertyChanged("MaterialCost");
            NotifyPropertyChanged("MaterialSalesPrice");
            NotifyPropertyChanged("MaterialContributionMargin");
            NotifyPropertyChanged("SalaryCost");
            NotifyPropertyChanged("SalarySalesPrice");
            NotifyPropertyChanged("SalaryContributionMargin");
            NotifyPropertyChanged("TotalCost");
            NotifyPropertyChanged("TotalSalesPrice");
            NotifyPropertyChanged("TotalContributionMargin");
            NotifyPropertyChanged("TotalSalesPricePlusTax");
        }

        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText);
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
                solution = value;
                foreach (Appliance item in solution.Appliances)
                {
                    item.PropertyChanged += AnyPropertyChanged;
                }
            }
        }
        
        public double ApplianceCost { get { return Solution.Appliances.Select(x => x.Cost).Sum(); } }
        public double ApplianceSalesPrice { get { return Solution.Appliances.Select(x => x.SalesPrice).Sum(); } }
        public double ApplianceContributionMargin { get { return ApplianceSalesPrice - ApplianceCost; } }

        public ObservableCollection<Salary> Salaries = new ObservableCollection<Salary>();
        public double SalaryCost { get { return Salaries.Select(x => x.Cost).Sum(); } }
        public double SalarySalesPrice { get { return Salaries.Select(x => x.SalesPrice).Sum(); } }
        public double SalaryContributionMargin { get { return SalarySalesPrice - SalaryCost; } }

        public ObservableCollection<Material> Materials = new ObservableCollection<Material>();
        public double MaterialCost { get { return Materials.Select(x => x.Cost).Sum(); } }
        public double MaterialSalesPrice { get { return Materials.Select(x => x.SalesPrice).Sum(); } }
        public double MaterialContributionMargin { get { return MaterialSalesPrice - MaterialCost; } }

        public double TotalCost { get { return MaterialCost + SalaryCost + ApplianceCost; } }
        public double TotalSalesPrice { get { return MaterialSalesPrice + SalarySalesPrice + ApplianceSalesPrice; } }
        public double TotalContributionMargin { get { return MaterialContributionMargin + SalaryContributionMargin + ApplianceContributionMargin; } }
        public double TotalSalesPricePlusTax { get { return TotalSalesPrice * 1.25; } }

        public string Date { get; set; }

        private static int count = 1;
        public readonly int Counter;

    }
}
