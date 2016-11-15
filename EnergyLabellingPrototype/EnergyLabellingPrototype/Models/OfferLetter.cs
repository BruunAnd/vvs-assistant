using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLabellingPrototype.Models
{
    class OfferLetter
    {
        public OfferLetter(Offer offer)
        {
            Offer = offer;
        }
        public Offer Offer { get; set; }
        public string LetterOpening { get; set; }
        public string LetterEnding { get; set; }
        public bool ShowPrice { get; set; }
    }
}
