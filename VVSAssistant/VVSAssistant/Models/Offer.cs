using System;
using System.Collections.Generic;
using System.Linq;

namespace VVSAssistant.Models
{
    public class Offer
    {
        public Offer()
        {
            Salaries = new List<UnitPrice>();
            Materials = new List<UnitPrice>();
            Appliances = new List<UnitPrice>();
        }

        public double TotalSalesPrice => Salaries.Sum(salary => salary.SalesPrice)
                                         + Materials.Sum(material => material.SalesPrice)
                                         + Appliances.Sum(offeredAppliance => offeredAppliance.SalesPrice);

        public double AppliancesSalesPrice => Appliances.Sum(appliance => appliance.SalesPrice);
        public double MaterialsSalesPrice => Materials.Sum(material => material.SalesPrice);
        public double SalariesSalesPrice => Salaries.Sum(salary => salary.SalesPrice);

        public double TotalCostPrice { get; set; } //Set in CreateOfferVM
        public double TotalContributionMargin { get; set; } //Set in CreateOfferVM

        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public Client Client { get; set; }
        public OfferInformation OfferInformation { get; set; }
        public PackagedSolution PackagedSolution { get; set; }
        public ICollection<UnitPrice> Salaries { get; set; }
        public ICollection<UnitPrice> Materials { get; set; }
        public ICollection<UnitPrice> Appliances { get; set; }
    }
}
