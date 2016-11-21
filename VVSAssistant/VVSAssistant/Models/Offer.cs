using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Offer
    {
        public Offer()
        {
            Salaries = new List<Salary>();
            Materials = new List<Material>();
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual OfferInformation OfferInformation { get; set; }
        public virtual PackagedSolution PackagedSolution { get; set; }
        public virtual ICollection<Salary> Salaries { get; }
        public virtual ICollection<Material> Materials { get; }
    }
}
