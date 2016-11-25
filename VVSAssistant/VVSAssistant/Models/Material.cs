using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Material : UnitPrice
    {
        public string VvsNumber { get; set; }

        public string SpecificationsType { get; set; }

        public string Name { get; set; }
    }
}
