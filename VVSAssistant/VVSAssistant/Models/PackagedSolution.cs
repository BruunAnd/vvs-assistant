using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class PackagedSolution
    {
        /* Lazy loading has been removed - should it be used? */
        public PackagedSolution()
        {
            Appliances = new List<Appliance>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Appliance> Appliances { get; set; }
    }
}
