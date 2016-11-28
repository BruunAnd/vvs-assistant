using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models
{
    public class MaterialReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VvsNumber { get; set; }
        public string SpecificationsType { get; set; }
        public double CostPrice { get; set; }
    }
}
