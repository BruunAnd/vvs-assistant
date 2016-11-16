using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace EnergyLabellingPrototype.Models
{
    public class Offer : OfferPrint, IFilterable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Offer()
        {
            Date = DateTime.Now.ToString();
            ID = count;
            count++;
            
            Materials.CollectionChanged += OnCollectionChanged;
            Salaries.CollectionChanged += OnCollectionChanged;
            Appliances.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (UnitPrice item in e.NewItems)
                {
                    item.PropertyChanged += OnPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (UnitPrice item in e.OldItems)
                {
                    item.PropertyChanged -= OnPropertyChanged;
                }
            }

            NotifyPropertyChanged();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged();
        }


        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText);
        }

        public string Name { get; set; }
        
        public ObservableCollection<Appliance> Appliances = new ObservableCollection<Appliance>();
        public ObservableCollection<Salary> Salaries = new ObservableCollection<Salary>();
        public ObservableCollection<Material> Materials = new ObservableCollection<Material>();
        
        public double TotalCostPrice { get { return Materials.TotalCostPrice() + Salaries.TotalCostPrice() + Appliances.TotalCostPrice(); } }
        public double TotalSalesPrice { get { return Materials.TotalSalesPrice() + Salaries.TotalSalesPrice() + Appliances.TotalSalesPrice(); } }
        public double TotalContributionMargin { get { return Materials.TotalContributionMargin() + Salaries.TotalContributionMargin() + Appliances.TotalContributionMargin(); } }
        public double TotalSalesPricePlusTax { get { return TotalSalesPrice * 1.25; } }

        public string Date { get; set; }

        private static int count = 1;
        public int ID { get; private set; }
    }
}
