using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VVSAssistant.Models
{
    public class PackagedSolution
    {
        /* Lazy loading has been removed - should it be used? */
        public PackagedSolution()
        {
            Appliances = new List<Appliance>();
        }

        protected Appliance _primaryHeatingUnit;
        //[NotMapped]
        public Appliance PrimaryHeatingUnit { get { return _primaryHeatingUnit; }
            set
            {
                _primaryHeatingUnit = value;
                Appliances.Add(_primaryHeatingUnit);
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Appliance> Appliances { get; set; }
    }
}
