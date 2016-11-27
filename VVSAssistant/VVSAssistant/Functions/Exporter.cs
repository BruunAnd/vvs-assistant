using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.Views;

namespace VVSAssistant.Functions
{
    internal class Exporter
    {
        private PdfLabelExportViewModel _vm;
        public void ExportOffer(Offer offer)
        {

        }

        public void ExportEnergyLabel(PackagedSolution packaged)
        {
            var cm = new CalculationManager();
            var cal = cm.SelectCalculationStrategy(packaged);

            var result = cal.Select(variable => variable.CalculateEEI(packaged)).ToList();


            Console.WriteLine("Print pdf == Done");
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

            const string path = "PdfEnergylabel.xps";
            var fixedDoc = new FixedDocument();
            var pageContent = new PageContent();
            var fixedPage = new FixedPage
            {
                Height = 11.69*96,
                Width = 8.27*96
            };
            var pageSize = new Size(8.5 * 96.0, 11.0 * 96.0);

            var v = new PdfLabelLayout();
            _vm = new PdfLabelExportViewModel();

            

            switch (result.Count)
            {
                case 1: SetUpLabelTwo(packaged ,result); break;
                case 2: SetUpLabelOne(packaged, result[0]); break;
                default: return;
            }


            v.DataContext = _vm;
            v.UpdateLayout();
            v.Height = pageSize.Height;
            v.Width = pageSize.Width;
            v.UpdateLayout();

            fixedPage.Children.Add(v);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
            //Ny side
            fixedDoc.Pages.Add(pageContent);

            if (File.Exists(path))
                File.Delete(path);
            var xpsd = new XpsDocument(path, FileAccess.ReadWrite);
            var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDoc);
            xpsd.Close();
        }

        private void SetUpLabelTwo(PackagedSolution packaged, IReadOnlyList<EEICalculationResult> result)
        {
            _vm.CompNameText = "Hjøring VVS";
            _vm.LabelOne = (false) ? "Visible" : "Collapsed";
            _vm.LabelTwo = (true) ? "Visible" : "Collapsed";

            _vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            _vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            _vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            _vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            _vm.TapValue = result[1].WaterHeatingUseProfile.ToString();
            SelectArrowValue(1, result[0].EEICharacters);
            SelectArrowValue(2, result[1].EEICharacters);
            SelectArrowValue(3, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE));
            SelectArrowValue(4, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].WaterHeatingEffciency));
        }

        private void SetUpLabelOne(PackagedSolution packaged, EEICalculationResult result)
        {
            _vm.CompNameText = "Hjøring VVS";
            _vm.LabelOne = (true) ? "Visible" : "Collapsed";
            _vm.LabelTwo = (false) ? "Visible" : "Collapsed";

            _vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            _vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            _vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            _vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            SelectArrowValue(3, EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result.PrimaryHeatingUnitAFUE));
            SelectArrowValue(5, result.EEICharacters);
        }
        private void SelectArrowValue(int arrow, string label)
        {
            int arrowPlace;
            string letterOnArrow;
            var plusOnArrow = "";
            switch (label)
            {
                case "A+++": letterOnArrow = "A"; plusOnArrow = "+++"; arrowPlace = 1; break;
                case "A++": letterOnArrow = "A"; plusOnArrow = "++"; arrowPlace = 2; break;
                case "A+": letterOnArrow = "A"; plusOnArrow = "+"; arrowPlace = 3; break;
                case "A": letterOnArrow = "A"; arrowPlace = 4; break;
                case "B": letterOnArrow = "B"; arrowPlace = 5; break;
                case "C": letterOnArrow = "C"; arrowPlace = 6; break;
                case "D": letterOnArrow = "D"; arrowPlace = 7; break;
                case "E": letterOnArrow = "E"; arrowPlace = 8; break;
                case "F": letterOnArrow = "F"; arrowPlace = 9; break;
                case "G": letterOnArrow = "G"; arrowPlace = 10; break;
                default: return;
            }
            switch (arrow)
            {
                case 1:
                    SetArrowOneLabelTwo(arrowPlace, letterOnArrow, plusOnArrow);
                    break;
                case 2:
                    SetArrowTwoLabelTwo(arrowPlace, letterOnArrow, plusOnArrow);
                    break;
                case 3:
                    SetAnnualWaterHeatingEfficiency(letterOnArrow, plusOnArrow);
                    break;
                case 4:
                    SetAnnualHeatingEfficiency(letterOnArrow, plusOnArrow);
                    break;
                case 5:
                    SetEeiArrowLebelOne(arrowPlace, letterOnArrow, plusOnArrow);
                    break;
            }
        }
        private void SetArrowOneLabelTwo(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            _vm.tabeOneArrow = arrowPlace;
            _vm.tabeOneArrowLetter = arrowLetter;
            _vm.tabeOneArrowPlus = arrowPlus;
        }
        private void SetArrowTwoLabelTwo(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            _vm.tabeTwoArrow = arrowPlace;
            _vm.tabeTwoArrowLetter = arrowLetter;
            _vm.tabeTwoArrowPlus = arrowPlus;
        }
        private void SetAnnualWaterHeatingEfficiency(string arrowLetter, string arrowPlus)
        {
            _vm.waterHeatingModeLetter = arrowLetter;
            _vm.waterHeatingModePlus = arrowPlus;
        }
        private void SetAnnualHeatingEfficiency(string arrowLetter, string arrowPlus)
        {
            _vm.annualEfficiencyLetter = arrowLetter;
            _vm.annualEfficiencyPlus = arrowPlus;
        }
        private void SetEeiArrowLebelOne(int arrowPlace, string arrowLetter, string arrowPlus)
        {
            _vm.labelTwoTabeOneArrow = arrowPlace;
            _vm.labelTwoTabeOneArrowLetter = arrowLetter;
            _vm.labelTwoTabeOneArrowPlus = arrowPlus;
        }

    }
}
