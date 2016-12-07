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
using VVSAssistant.Database;
using VVSAssistant.Exceptions;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels



{

    class OfferExportViewModel
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


        public string Signatur { get; set; }
        public string CompanyName { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Telephone { get; set; }
        public string CVR { get; set; }

        public double TotalSalesPrice { get; set; }
        public double TotalSalesPriceInkl { get; set; }


        public string CreationDate { get; set; }


        public string OfferTitle { get; set; }
        public string ClientName { get; set; } 
        public string ClientStreet { get; set; }
        public string ClientCity { get; set; }
        public string ClientCompanyName { get; set; }

        public string IntroText { get; set; }
        public string OutroText { get; set; }
        public string Moms { get; set; }
        public double MomsAmount { get; set; }
        public double TotalAmountEks { get; set; }
        public double AppliancesPrice { get; set; }
        public double SalariesPrice { get; set; }
        public double MaterialsPrice { get; set; }
        public void Setup(Offer offer)
        {
            OfferTitle = offer.OfferInformation.Title;
            ClientCompanyName = offer.Client.ClientInformation.CompanyName;
            ClientName = offer.Client.ClientInformation.Name;
            ClientStreet = offer.Client.ClientInformation.Address;
            ClientCity = offer.Client.ClientInformation.City + ", " + offer.Client.ClientInformation.PostalCode;

            CreationDate = offer.CreationDate.ToString("dd. MMMM yyyy");
            Signatur = offer.OfferInformation.Signature;

            using (var ctx = new AssistantContext())
            {
                var companyInfo = ctx.CompanyInformation.FirstOrDefault();
                if (companyInfo == null)
                    throw new CompanyInformationNotFoundException("Ingen firmaoplysninger er indtastet.");

                CompanyName = companyInfo.CompanyName;
                Adresse = companyInfo.Address;
                Email = companyInfo.Email;
                WebSite = companyInfo.Website;
                Telephone = companyInfo.Telephone;
                CVR = companyInfo.Cvr;
            }

            IntroText = offer.OfferInformation.Intro;
            OutroText = offer.OfferInformation.Outro;

            TotalAmountEks = offer.TotalSalesPrice;
            TotalSalesPriceInkl = (offer.TotalSalesPrice*1.25);
            MomsAmount = TotalSalesPriceInkl - offer.TotalSalesPrice;
            Moms = ((offer.OfferInformation.ApplyTax) ? "inkl. moms" : "Eks. moms");
            TotalSalesPrice = ((offer.OfferInformation.ApplyTax) ? TotalSalesPriceInkl : TotalAmountEks);

            AppliancesPrice = offer.AppliancesSalesPrice;
            SalariesPrice = offer.SalariesSalesPrice;
            MaterialsPrice = offer.MaterialsSalesPrice;
        }
    }
}
