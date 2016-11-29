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

namespace VVSAssistant.ViewModels
{
    class PdfLabelExportViewModel
    {
        public string LabelOne { get; set; }
        public string CompNameText { get; set;}
        public string TapValue { get; set; }
        public string SolarIncluded { get; set; }
        public string WatertankIncluded { get; set; }
        public string TempControleIncluded { get; set; }
        public string HeaterIncluded { get; set; }

        private int _tabelOneArrow;
        public int TabelOneArrow
        {
            get { return _tabelOneArrow + 1; }
            set { _tabelOneArrow = value; }
        }
        private int _tabelTwoArrow;
        public int TabelTwoArrow
        {
            get { return _tabelTwoArrow + 1; }
            set {
                {
                    _tabelTwoArrow = value;
                }
            }
        }
        public string TabelOneArrowLetter { get; set; }
        public string TabelOneArrowPlus { get; set; }
        public string TabelTwoArrowLetter { get; set; }
        public string TabelTwoArrowPlus { get; set; }
        
        public string WaterHeatingModeLetter { get; set; }
        public string WaterHeatingModePlus { get; set; }
        public string AnnualEfficiencyLetter { get; set; }
        public string AnnualEfficiencyPlus { get; set; }


        public string LabelTwo { get; set; }
        public int LabelTwoTabeOneArrow { get; set; }
        public string LabelTwoTabeOneArrowLetter { get; set; }
        public string LabelTwoTabeOneArrowPlus { get; set; }

        public PdfLabelExportViewModel()
        {
            
        }
}
    }

