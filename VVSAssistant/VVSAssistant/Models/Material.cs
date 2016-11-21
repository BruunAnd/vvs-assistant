using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Material : UnitPrice
    {
        public int VvsNumber { get; set; }
        public string Name { get; set; }
    }
}
