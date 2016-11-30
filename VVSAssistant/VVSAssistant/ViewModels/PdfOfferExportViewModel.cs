using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    class PdfOfferExportViewModel
    {
        private string _pageOne;
        public string PageOne
        {
            get
            {
                return this._pageOne != null ? _pageOne : "Collapsed";
            }
            set { _pageOne = value; }
        }
        private string _pageTwo;
        public string PageTwo
        {
            get
            {
                return this._pageTwo != null ? _pageTwo : "Collapsed";
            }
            set { _pageTwo = value; }
        }

        private string _pageThree;
        public string PageThree
        {
            get
            {
                return this._pageThree != null ? _pageThree : "Collapsed";

            }
            set { _pageThree = value; }
        }

        public ApplianceList ApplianceList { get; set; }
        public ICollection<Material> MaterialsList { get; set; }
        public ICollection<Salary> SalaryList { get; set; }
        public DateTime CreationDate { get; set; } // Ikke helt færdig!
        public string OfferName { get; set; }
        public double TotalSalesPrice { get; set; }
        public string ClientName { get; set; }
        public string ClientAdresse { get; set; }
        public string ClientCity { get; set; }
        public string IntroText { get; set; }
        public string OutroText { get; set; }
        public string Moms { get; set; }

        public void SetUp(Offer offer)
        {
            ApplianceList = offer.PackagedSolution.Appliances;
            MaterialsList = offer.Materials;
            CreationDate = offer.CreationDate;
            OfferName = offer.OfferInformation.Title;
            ClientName = offer.Client.ClientInformation.Name;
            ClientAdresse = offer.Client.ClientInformation.Address;
            ClientCity = offer.Client.ClientInformation.Address; //TODO: ClientCity
            IntroText = offer.OfferInformation.Intro;
            OutroText = offer.OfferInformation.Outro;

            TotalSalesPrice = offer.TotalSalesPrice;
            Moms = ((offer.OfferInformation.ApplyTax) ? "inkl. moms" : "Eks. moms");
        }
    }
}
