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
        private ObservableCollection<Component> _components = new ObservableCollection<Component>();

        private static int _count = 1;
        public int Counter
        {
            get; set;
        }

        private string _name;
        public string Name
        {
            get { return _name; } set { SetProperty(ref _name, value);}
        }
        

        public string Date
        {
            get; set;
        }

        public ObservableCollection<Component> SolutionList
        {
            get
            {
                return _components;
            }
            set
            {
                _components = value;
            }
        }
        
        public string Info
        {
            get
            {
                return string.Join(", ", _components.Select(c => c.Name).ToArray());
            }
        }

        public Solution(string name, IEnumerable<Component> componentList)
        {
            foreach (var component in componentList) _components.Add(component);
            Date = DateTime.Now.ToString();
            Name = name + _count;
            Counter = _count;
            _count++;
        }

        public bool FilterMatch(string filterText)
        {
            if (Info.ToLower().Contains(filterText) || Name.ToLower().Contains(filterText))
                return true;

            foreach (Component component in _components)
                if (!component.FilterMatch(filterText))
                    return false;

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value,
           [CallerMemberName] string propname = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                var pc = PropertyChanged;
                pc?.Invoke(this, new PropertyChangedEventArgs(propname));
            }
        }
    }
}
