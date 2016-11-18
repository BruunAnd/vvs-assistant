using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Appliance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ApplianceTypes Type { get; set; }
        public virtual DataSheet DataSheet { get; set; }
    }
}
