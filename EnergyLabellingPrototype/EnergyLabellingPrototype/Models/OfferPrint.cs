using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    public abstract class OfferPrint
    {
        public string LetterOpening { get; set; }
        public string LetterEnding { get; set; }
        public bool ShowPrice { get; set; }
    }
}
