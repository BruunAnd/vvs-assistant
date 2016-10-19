using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype
{
    public class Component : IFilterable, INotifyPropertyChanged
    {
        private static int _count = 1;

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

        public Component(string name, string description, string type)
        {
            Name = name;
            Counter = _count;
            Description = description;
            Type = type;
            _count++;
        }

        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText) || Type.ToLower().Contains(filterText) || Description.ToLower().Contains(filterText);
        }

        public int Counter { get; set; }

        private string _name;
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }

        private string _description;
        public string Description { get { return _description; } set { SetProperty(ref _description, value); } }

        private string _type = null;
        public string Type { get { return _type; } set { SetProperty(ref _type, value); } }
    }
}
