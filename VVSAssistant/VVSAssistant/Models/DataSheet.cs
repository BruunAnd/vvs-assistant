using System;
using System.ComponentModel;
using System.Linq;
using VVSAssistant.Models.Interfaces;

namespace VVSAssistant.Models
{
    public abstract class DataSheet : ICopyable
    {
        [Browsable(false)]
        public int Id { get; set; }

        [DisplayName(@"Pris")]
        [Description(@"eksempel")]
        public double Price { get; set; }

        public virtual object MakeCopy()
        {
            var copy = Activator.CreateInstance(GetType());
            var properties = GetType().GetProperties();
            foreach (var pi in copy.GetType().GetProperties())
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name);
                pi.SetValue(copy, matchingProperty.GetValue(this));
            }
            return copy;
        }
    }
}
