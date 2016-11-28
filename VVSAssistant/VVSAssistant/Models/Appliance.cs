using System;
using System.Linq;
using System.Reflection;
using VVSAssistant.Functions.Calculation.Interfaces;
using VVSAssistant.Models.Interfaces;

namespace VVSAssistant.Models
{
    public class Appliance : UnitPrice, ICalculateable, ICopyable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplianceTypes Type { get; set; }
        public virtual DataSheet DataSheet { get; set; }
        public string Description => DataSheet.ToString();

        public Appliance(string name, DataSheet datasheet, ApplianceTypes type)
        {
            Name = name;
            DataSheet = datasheet;
            Type = type;
            Quantity = 1;
        }

        public Appliance()
        {
        }

        public override string ToString()
        {
            return Name;
        }
        public object MakeCopy()
        {
            var copy = Activator.CreateInstance(this.GetType()); //New object of same type
            var properties = this.GetType().GetProperties(); //All properties from this object
            foreach (PropertyInfo pi in copy.GetType().GetProperties()) //For all the properties in the new object
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name); //Find the property with the same name in this object
                pi.SetValue(copy, matchingProperty.GetValue(this)); //Set the new object's property with this name to the value of the same property in this object
            }
            return copy;
        }
    }
}
