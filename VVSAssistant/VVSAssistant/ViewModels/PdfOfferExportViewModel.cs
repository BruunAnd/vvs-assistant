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
        public string PageOne { get; set; }
        public string PageTwo { get; set; }
        public string PageThree { get; set; }



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
    }
}
