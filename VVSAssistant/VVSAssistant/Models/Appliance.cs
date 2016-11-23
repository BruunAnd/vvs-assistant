using System;
using System.Collections.Generic;
using VVSAssistant.Models.Interfaces;

namespace VVSAssistant.Models
{
    public class Appliance : ICalculateable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplianceTypes Type { get; set; }
        public virtual DataSheet DataSheet { get; set; }
        public virtual UnitPrice UnitPrice { get; set; }
    }
}
