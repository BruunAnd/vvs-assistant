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

        private int _tabeOneArrow;
        public int tabeOneArrow
        {
            get { return _tabeOneArrow + 1; }
            set { _tabeOneArrow = value; }
        }
        private int _tabeTwoArrow;
        public int tabeTwoArrow
        {
            get { return _tabeTwoArrow + 1; }
            set {
                {
                    _tabeTwoArrow = value;
                }
            }
        }
        public string tabeOneArrowLetter { get; set; }
        public string tabeOneArrowPlus { get; set; }
        public string tabeTwoArrowLetter { get; set; }
        public string tabeTwoArrowPlus { get; set; }
        
        public string waterHeatingModeLetter { get; set; }
        public string waterHeatingModePlus { get; set; }
        public string annualEfficiencyLetter { get; set; }
        public string annualEfficiencyPlus { get; set; }


        public string LabelTwo { get; set; }
        public int labelTwoTabeOneArrow { get; set; }
        public string labelTwoTabeOneArrowLetter { get; set; }
        public string labelTwoTabeOneArrowPlus { get; set; }

        public PdfLabelExportViewModel()
        {
            
        }
}
    }

