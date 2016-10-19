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
        private static int _count = 1;

        private ObservableCollection<Component> _solutionList = new ObservableCollection<Component>();

        private string _info;

        public int Counter
        {
            get; set;
        }

        private string _name;
        public string Name
        {
            get { return _name; } set { SetProperty(ref _name, value);}
        }

        public string Type
        {
            get; set;
        }

        public string Date
        {
            get; set;
        }

        public ObservableCollection<Component> SolutionList
        {
            get
            {
                return _solutionList;
            }
            set
            {
                _solutionList = value;
            }
        }

        public string Information
        {
            get
            {
                return _info ?? string.Join(", ", SolutionList.Select(c => c.Type));
            }
            set
            {
                _info = value;
            }
        }

        public Solution(string name , IEnumerable<Component> list)
        {
            foreach (var i in list) _solutionList.Add(i);
            Date = DateTime.Now.ToString();
            Name = name + _count;
            Counter = _count;
            Type = "Pakke";
            _count++;
        }

        public bool FilterMatch(string filterText)
        {
            if (Information.ToLower().Contains(filterText) || Name.ToLower().Contains(filterText))
                return true;

            foreach (Component component in SolutionList)
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
