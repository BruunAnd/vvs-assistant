using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using VVSAssistant.Functions.Calculation.Interfaces;
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
            var copy = Activator.CreateInstance(this.GetType());
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo pi in copy.GetType().GetProperties())
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name);
                pi.SetValue(copy, matchingProperty.GetValue(this));
            }
            return copy;
        }
    }
}
