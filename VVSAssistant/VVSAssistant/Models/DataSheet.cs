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
        [Description(@"Prisen for komponentet i danske kroner")]
        public double Price { get; set; }

        public virtual object MakeCopy()
        {
            var copy = Activator.CreateInstance(this.GetType()); //New object of same type
            var properties = this.GetType().GetProperties(); //All properties from this object
            foreach (var pi in copy.GetType().GetProperties()) //For all the properties in the new object
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name); //Find the property with the same name in this object
                pi.SetValue(copy, matchingProperty.GetValue(this)); //Set the new object's property with this name to the value of the same property in this object
            }
            return copy;
        }
    }
}
