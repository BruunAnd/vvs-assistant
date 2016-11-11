using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype
{
    public class Solution : IFilterable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Solution(string name, IEnumerable<Component> componentList)
        {
            foreach (var component in componentList) components.Add(component);
            Date = DateTime.Now.ToString();
            Name = name;
            Counter = count;
            count++;
        }

        public bool FilterMatch(string filterText)
        {
            if (Info.ToLower().Contains(filterText) || Name.ToLower().Contains(filterText))
                return true;

            foreach (Component component in components)
                if (!component.FilterMatch(filterText))
                    return false;

            return true;
        }

        private ObservableCollection<Component> components = new ObservableCollection<Component>();
        public ObservableCollection<Component> Components
        {
            get
            {
                return components;
            }
            set
            {
                if(value != components)
                {
                    components = value;
                    NotifyPropertyChanged();
                }
            }
        }

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
                return string.Join(", ", components.Select(c => c.Name).ToArray());
            }
        }

        public string Date
        {
            get; set;
        }

        private static int count = 1;
        public readonly int Counter;
    }
}
