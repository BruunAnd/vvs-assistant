using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype.Models
{
    public class Solution : IFilterable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Solution(string name, IEnumerable<Appliance> appliances)
        {
            foreach (var appliance in appliances) Appliances.Add(appliance);
            Date = DateTime.Now.ToString();
            Name = name;
            ID = counter;
            counter++;
        }

        public bool FilterMatch(string filterText)
        {
            if (Info.ToLower().Contains(filterText) || Name.ToLower().Contains(filterText))
                return true;

            foreach (Appliance component in Appliances)
                if (!component.FilterMatch(filterText))
                    return false;

            return true;
        }

        public ObservableCollection<Appliance> Appliances = new ObservableCollection<Appliance>();

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Info
        {
            get
            {
                return string.Join(", ", Appliances.Select(c => c.Name).ToArray());
            }
        }

        public string Date
        {
            get; set;
        }

        public int ID { get; private set; }

        // Count Solution instances, starting at 1.
        private static int counter = 1;
    }
}
