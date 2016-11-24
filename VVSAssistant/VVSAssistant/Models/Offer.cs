using System;
using System.Collections.Generic;
using System.Linq;

namespace VVSAssistant.Models
{
    public class Offer
    {
        public Offer()
        {
            Salaries = new List<Salary>();
            Materials = new List<Material>();
        }

        public double TotalSalesPrice => Salaries.Sum(salary => salary.SalesPrice)
                                         + Materials.Sum(material => material.SalesPrice)
                                         + PackagedSolution.Appliances.Sum(appliance => appliance.UnitPrice.SalesPrice);

        public double TotalCostPrice => Salaries.Sum(salary => salary.SalesPrice)
                                        + Materials.Sum(material => material.SalesPrice)
                                        + PackagedSolution.Appliances.Sum(appliance => appliance.UnitPrice.SalesPrice);

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual OfferInformation OfferInformation { get; set; }
        public virtual PackagedSolution PackagedSolution { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
