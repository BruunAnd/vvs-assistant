using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VVSAssistant.Models.Interfaces;
using VVSAssistant.ValueConverters;

namespace VVSAssistant.Models
{
    public class Appliance : ICalculable, IEquatable<Appliance>
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
