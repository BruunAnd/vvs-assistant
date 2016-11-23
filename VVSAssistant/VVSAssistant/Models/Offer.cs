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
            Client = new Client();
            OfferInformation = new OfferInformation();
            PackagedSolution = new PackagedSolution();
            CreationDate = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual OfferInformation OfferInformation { get; set; }
        public virtual PackagedSolution PackagedSolution { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Material> Materials { get; set;  }
    }
}
