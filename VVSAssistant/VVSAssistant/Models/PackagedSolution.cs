using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VVSAssistant.Models
{
    public class PackagedSolution
    {
        public PackagedSolution()
        {
            Appliances = new List<Appliance>();
        }

        public Appliance SolarContainer;

        protected Appliance _primaryHeatingUnit;
        //[NotMapped]
        public virtual Appliance PrimaryHeatingUnit
        {
            get { return _primaryHeatingUnit; }
            set
            {
                _primaryHeatingUnit = value;
                Appliances.Add(_primaryHeatingUnit);
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<Appliance> Appliances { get; }
    }
}
