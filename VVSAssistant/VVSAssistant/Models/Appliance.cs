using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VVSAssistant.Models.Interfaces;
using VVSAssistant.ValueConverters;

namespace VVSAssistant.Models
{
    public class Appliance : ICalculable, ICopyable, IEquatable<Appliance>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplianceTypes Type { get; set; }
        public int? DataSheetId { get; set; }
        [ForeignKey("DataSheetId")]
        public DataSheet DataSheet { get; set; }
        public string Description => DataSheet.ToString();
        public double UnitCostPrice { get; set; }

        public Appliance(string name, DataSheet datasheet, ApplianceTypes type)
        {
            Name = name;
            DataSheet = datasheet;
            Type = type;
        }

        public Appliance()
        {
        }

        public override string ToString()
        {
            return $"{Name} ({new ApplianceTypeConverter().Convert(Type, null, null, null)})";
        }

        public object MakeCopy()
        {
            var copy = Activator.CreateInstance(this.GetType()); //New object of same type
            var properties = GetType().GetProperties(); //All properties from this object
            foreach (var pi in copy.GetType().GetProperties()) //For all the properties in the new object
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name); //Find the property with the same name in this object
                if (pi == this.GetType().GetProperty("Description")) // Description does not have a set method
                    continue;
                else
                    pi.SetValue(copy, matchingProperty.GetValue(this)); //Set the new object's property with this name to the value of the same property in this object
            }
            return copy;
        }

        public bool Equals(Appliance other)
        {
            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            int appName = Name == null ? 0 : Name.GetHashCode();
            int creationDate = CreationDate == null ? 0 : CreationDate.GetHashCode();
            return appName ^ creationDate;
        }
    }
}
