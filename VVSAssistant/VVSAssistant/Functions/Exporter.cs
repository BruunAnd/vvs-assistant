using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;
using VVSAssistant.Views;

namespace VVSAssistant.Functions
{
    class Exporter
    {
        private PdfLabelExportViewModel vm;
        public void ExportOffer()
        {

            Console.WriteLine("Print Offer pdf == Done");


        var path = "PdfOffer.xps";
        FixedDocument fixedDoc = new FixedDocument();
        PageContent pageContent = new PageContent();
        FixedPage fixedPage = new FixedPage();
        fixedPage.Height = 11.69 * 96;
        fixedPage.Width = 8.27 * 96;
            var pageSize = new Size(8.5 * 96.0, 11.0 * 96.0);
            PdfOfferLayout v = new PdfOfferLayout();
        PdfOfferExportViewModel vmOffer = new PdfOfferExportViewModel();

        v.DataContext = vmOffer;
        v.UpdateLayout();
        v.Height = pageSize.Height;
        v.Width = pageSize.Width;
        v.UpdateLayout();
        
        fixedPage.Children.Add(v);
        ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
        fixedDoc.Pages.Add(pageContent);


            if (File.Exists(path))
            File.Delete(path);
        XpsDocument xpsd = new XpsDocument(path, FileAccess.ReadWrite);
        XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
        xw.Write(fixedDoc);
        xpsd.Close();

    }

        public void ExportEnergyLabel(PackagedSolution packaged)
        {
            List<EEICalculationResult> result = new List<EEICalculationResult>();

            CalculationManager cm = new CalculationManager();
            var cal = cm.SelectCalculationStrategy(packaged);
            if (cal == null)
            {
                return;
            }
            foreach (var VARIABLE in cal)
            {
                result.Add(VARIABLE.CalculateEEI(packaged));
            }
            

            Console.WriteLine("Print Label pdf == Done");
            Console.WriteLine("Count " + result.Count);
            //Console.WriteLine("Index 0 " + result[0].WaterHeatingUseProfile);
            //Console.WriteLine(result[0].PrimaryHeatingUnitAFUE);
            Console.WriteLine("Main for index 0 "+result[0].EEICharacters);
            Console.WriteLine(result[0].EEI);
            //Console.WriteLine(result[0].WaterHeatingEffciency);
            Console.WriteLine("EEI " + EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE));
            Console.WriteLine("EEI " + EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].WaterHeatingEffciency));

            if (result.Count > 1)
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Index 1 " + result[1].WaterHeatingUseProfile);
                //Console.WriteLine(result[1].PrimaryHeatingUnitAFUE);
                Console.WriteLine("Main for index 1 " + result[1].EEICharacters);
                Console.WriteLine(result[1].EEI);
                //Console.WriteLine(result[1].WaterHeatingEffciency);
                Console.WriteLine("EEI Heat " + EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].PrimaryHeatingUnitAFUE));
                Console.WriteLine("EEI Water " + EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].WaterHeatingEffciency));
            }

            var path = "PdfEnergylabel.xps";
            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();
            fixedPage.Height = 11.69 * 96;
            fixedPage.Width = 8.27 * 96;
            var pageSize = new Size(8.5 * 96.0, 11.0 * 96.0);

            PdfLabelLayout v = new PdfLabelLayout();
            vm = new PdfLabelExportViewModel();

            

            switch (result.Count)
            {
                case 2: SetUpLabelTwo(packaged ,result); break;
                case 1: SetUpLabelOne(packaged, result[0]); break;
                default: return;
            }


            v.DataContext = vm;
            v.UpdateLayout();
            v.Height = pageSize.Height;
            v.Width = pageSize.Width;
            v.UpdateLayout();

            fixedPage.Children.Add(v);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            if (File.Exists(path))
                File.Delete(path);
            XpsDocument xpsd = new XpsDocument(path, FileAccess.ReadWrite);
            XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDoc);
            xpsd.Close();
        }

        private void SetUpLabelTwo(PackagedSolution packaged, List<EEICalculationResult> result)
        {
            vm.CompNameText = "Hjøring VVS";
            vm.LabelOne = (false) ? "Visible" : "Collapsed";
            vm.LabelTwo = (true) ? "Visible" : "Collapsed";

            vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            vm.TapValue = result[1].WaterHeatingUseProfile.ToString();
            SelectArrowValue(1, result[0].EEICharacters);
            SelectArrowValue(2, result[1].EEICharacters);
            SelectArrowValue(3, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE));
            SelectArrowValue(4, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].WaterHeatingEffciency));
        }

        private void SetUpLabelOne(PackagedSolution packaged, EEICalculationResult result)
        {
            vm.CompNameText = "Hjøring VVS";
            vm.LabelOne = (true) ? "Visible" : "Collapsed";
            vm.LabelTwo = (false) ? "Visible" : "Collapsed";

            vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            SelectArrowValue(3, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result.PrimaryHeatingUnitAFUE));
            SelectArrowValue(5, result.EEICharacters);
        }
        private void SelectArrowValue(int arrow, string label)
        {
            int arrowPlace;
            string letterOnArrow;
            string PlusOnArrow = "";
            switch (label)
            {
                case "A+++": letterOnArrow = "A"; PlusOnArrow = "+++"; arrowPlace = 1; break;
                case "A++": letterOnArrow = "A"; PlusOnArrow = "++"; arrowPlace = 2; break;
                case "A+": letterOnArrow = "A"; PlusOnArrow = "+"; arrowPlace = 3; break;
                case "A": letterOnArrow = "A"; arrowPlace = 4; break;
                case "B": letterOnArrow = "B"; arrowPlace = 5; break;
                case "C": letterOnArrow = "C"; arrowPlace = 6; break;
                case "D": letterOnArrow = "D"; arrowPlace = 7; break;
                case "E": letterOnArrow = "E"; arrowPlace = 8; break;
                case "F": letterOnArrow = "F"; arrowPlace = 9; break;
                case "G": letterOnArrow = "G"; arrowPlace = 10; break;
                default: return;
            }
            if (arrow == 1)
            {
                SetArrowOneLabelTwo(arrowPlace, letterOnArrow, PlusOnArrow);
            }
            else if (arrow == 2)
            {
                SetArrowTwoLabelTwo(arrowPlace, letterOnArrow, PlusOnArrow);
            }
            else if (arrow == 3)
            {
                SetAnnualWaterHeatingEfficiency(letterOnArrow, PlusOnArrow);
            }
            else if (arrow == 4)
            {
                SetAnnualHeatingEfficiency(letterOnArrow, PlusOnArrow);
            }
            else if (arrow == 5)
            {
                SetEEIArrowLebelOne(arrowPlace, letterOnArrow, PlusOnArrow);
            }
        }
        private void SetArrowOneLabelTwo(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            vm.tabeOneArrow = arrowPlace;
            vm.tabeOneArrowLetter = arrowLetter;
            vm.tabeOneArrowPlus = arrowPlus;
        }
        private void SetArrowTwoLabelTwo(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            vm.tabeTwoArrow = arrowPlace;
            vm.tabeTwoArrowLetter = arrowLetter;
            vm.tabeTwoArrowPlus = arrowPlus;
        }
        private void SetAnnualWaterHeatingEfficiency(string arrowLetter, string arrowPlus)
        {
            vm.waterHeatingModeLetter = arrowLetter;
            vm.waterHeatingModePlus = arrowPlus;
        }
        private void SetAnnualHeatingEfficiency(string arrowLetter, string arrowPlus)
        {
            vm.annualEfficiencyLetter = arrowLetter;
            vm.annualEfficiencyPlus = arrowPlus;
        }
        private void SetEEIArrowLebelOne(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            vm.labelTwoTabeOneArrow = arrowPlace;
            vm.labelTwoTabeOneArrowLetter = arrowLetter;
            vm.labelTwoTabeOneArrowPlus = arrowPlus;
        }

    }
}
