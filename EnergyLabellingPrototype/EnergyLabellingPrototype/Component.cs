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

        public Component(string name, string description, string type, int price)
        {
            Name = name;
            Counter = _count;
            Description = description;
            Type = type;
            Price = price;
            _count++;
        }

        public bool FilterMatch(string filterText)
        {
            return Name.ToLower().Contains(filterText) || Type.ToLower().Contains(filterText) || Description.ToLower().Contains(filterText);
        }

        public int Counter { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
    }
}
