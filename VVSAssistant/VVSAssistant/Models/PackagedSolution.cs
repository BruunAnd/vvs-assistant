using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VVSAssistant.Models
{
    public class PackagedSolution
    {
        public PackagedSolution()
        {
            ApplianceInstances = new List<ApplianceInstance>();
        }

        [NotMapped]
        public Appliance SolarContainer
        {
            get { return SolarContainerInstance?.Appliance; }    
            set { SolarContainerInstance = new ApplianceInstance(value);}
        }

        [NotMapped]
        public Appliance PrimaryHeatingUnit
        {
            get { return PrimaryHeatingUnitInstance?.Appliance; }
            set { PrimaryHeatingUnitInstance = new ApplianceInstance(value); }
        }

        private ApplianceList _applianceList;
        [NotMapped]
        public ApplianceList Appliances
        {
            get { return _applianceList ?? (_applianceList = new ApplianceList((List<ApplianceInstance>) ApplianceInstances)); }
            set
            {
                _applianceList = value;
                ApplianceInstances = _applianceList.BackingList;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ApplianceInstance> ApplianceInstances { get; set; }
        public virtual ApplianceInstance SolarContainerInstance { get; private set; }
        public virtual ApplianceInstance PrimaryHeatingUnitInstance { get; private set; }
        public string Description => string.Join(", ", Appliances);
    }
}
