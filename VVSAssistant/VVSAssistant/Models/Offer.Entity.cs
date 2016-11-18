using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Offer
    {
        /* Lazy loading has been removed - should it be used? */
        public Offer()
        {
            Salaries = new List<Salary>();
            Materials = new List<Material>();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public Client Client { get; set; }
        public OfferInformation OfferInformation { get; set; }
        public PackagedSolution PackagedSolution { get; set; }
        public ICollection<Salary> Salaries { get; set; }
        public ICollection<Material> Materials { get; set; }
    }
}
