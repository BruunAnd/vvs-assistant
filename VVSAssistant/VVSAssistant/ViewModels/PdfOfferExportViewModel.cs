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



        public ApplianceList applianceList { get; set; }
        public ICollection<Material> materialsList { get; set; }
        public DateTime creationDate { get; set; } // Ikke helt færdig!
        public string offerName { get; set; }
        public double totalSalesPrice { get; set; }
    }
}
