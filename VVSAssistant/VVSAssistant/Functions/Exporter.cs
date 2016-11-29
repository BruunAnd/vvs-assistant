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
using System.Windows.Forms.VisualStyles;
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
        struct ArrowData
        {
            public int location { get; set; }
            public string Letter { get; set; }
            public string Plus { get; set; }
        }

        public void ExportOffer(Offer offer)
        {

            Console.WriteLine("Print Offer pdf == Done");
            Console.WriteLine(offer.PackagedSolution.Appliances.Count);
            var path = $"PdfOffer{offer.OfferInformation.Title}.xps"; //TODO: Throw an exception when the name already exists, ask for another name
            FixedDocument fixedDoc = new FixedDocument();
            PdfOfferExportViewModel vmOffer = new PdfOfferExportViewModel();
            vmOffer.ApplianceList = offer.PackagedSolution.Appliances;
            vmOffer.MaterialsList = offer.Materials;
            vmOffer.CreationDate = offer.CreationDate;
            vmOffer.OfferName = offer.OfferInformation.Title;
            vmOffer.ClientName = offer.Client.ClientInformation.Name;
            vmOffer.ClientAdresse = offer.Client.ClientInformation.Address;
            vmOffer.ClientCity = offer.Client.ClientInformation.Address; //TODO: ClientCity
            vmOffer.IntroText = offer.OfferInformation.Intro;
            vmOffer.OutroText = offer.OfferInformation.Outro;
        
            vmOffer.TotalSalesPrice = offer.TotalSalesPrice;
            vmOffer.Moms = ((offer.OfferInformation.ApplyTax) ? "inkl. moms" : "Eks. moms");
            #region PageOneToThree
            //PageOne
            PdfOfferLayout pageOne = new PdfOfferLayout();
            vmOffer.PageOne = (true) ? "Visible" : "Collapsed";
            vmOffer.PageTwo = (false) ? "Visible" : "Collapsed";
            vmOffer.PageThree = (false) ? "Visible" : "Collapsed";
            fixedDoc.Pages.Add(createPageContent(pageOne, vmOffer));

            //PageTwo
            PdfOfferLayout pageTwo = new PdfOfferLayout();
            vmOffer.PageOne = (false) ? "Visible" : "Collapsed";
            vmOffer.PageTwo = (true) ? "Visible" : "Collapsed";
            vmOffer.PageThree = (false) ? "Visible" : "Collapsed";
            fixedDoc.Pages.Add(createPageContent(pageTwo, vmOffer));

            //pageThree
            PdfOfferLayout pageThree = new PdfOfferLayout();
            vmOffer.PageOne = (false) ? "Visible" : "Collapsed";
            vmOffer.PageTwo = (false) ? "Visible" : "Collapsed";
            vmOffer.PageThree = (true) ? "Visible" : "Collapsed";
            fixedDoc.Pages.Add(createPageContent(pageThree, vmOffer));
            #endregion

            SaveXpsFile(path, fixedDoc);

        }

        private PageContent createPageContent<T,U>(T view, U vm) where T: ContentControl
        {
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();
            fixedPage.Height = 11.69 * 96;
            fixedPage.Width = 8.27 * 96;
            var pageSize = new Size(8.5 * 96.0, 11.0 * 96.0);

            view.DataContext = vm;
            view.UpdateLayout();
            view.Height = pageSize.Height;
            view.Width = pageSize.Width;
            view.UpdateLayout();
            fixedPage.Children.Add(view);
            ((IAddChild)pageContent).AddChild(fixedPage);
            return pageContent;
        }

        private void SaveXpsFile(string path, FixedDocument fixedDocument)
        {

            if (File.Exists(path))
                File.Delete(path); //TODO: Notify that the user should close all PDFs before doing this
            XpsDocument xpsd = new XpsDocument(path, FileAccess.ReadWrite);
            XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDocument);
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
#region Debug
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
#endregion
            var path = "PdfEnergylabel.xps";
            FixedDocument fixedDoc = new FixedDocument();
            PdfLabelLayout v = new PdfLabelLayout();
            PdfLabelExportViewModel vm = new PdfLabelExportViewModel();

            switch (result.Count)
            {
                case 2: SetUpLabelTwo(packaged ,result, vm); break;
                case 1: SetUpLabelOne(packaged, result[0], vm); break;
                default: return;
            }
            fixedDoc.Pages.Add(createPageContent(v, vm));

            PdfCalculationLayout calculationLayout = new PdfCalculationLayout();
            PdfCalculationViewModel calculationViewModel = new PdfCalculationViewModel();
            SetUpCalculationLayout(calculationViewModel, result[0]);
            fixedDoc.Pages.Add(createPageContent(calculationLayout, calculationViewModel));

            SaveXpsFile(path, fixedDoc);
        }

        private void SetUpCalculationLayout(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageOne = (false) ? "Visible" : "Collapsed";
            vm.PageTwo = (false) ? "Visible" : "Collapsed";
            vm.Pagethree = (false) ? "Visible" : "Collapsed";
            vm.PageFour = (false) ? "Visible" : "Collapsed";
            vm.PageFive = (false) ? "Visible" : "Collapsed";
            switch (result.CalculationType)
            {
                case CalculationType.PrimaryBoiler: CalPageTwo(vm, result); break;
                case CalculationType.PrimaryCPH: CalPageThree(vm, result); break;
                case CalculationType.PrimaryHeatPump: CalPageOne(vm, result); break;
                case CalculationType.PrimaryLowTempHeatPump: CalPageFour(vm, result); break;
                case CalculationType.PrimaryWaterBoiler: break;
                default: return;
            }

            vm.PrimAnnualEfficiency = CheckforNull(result.PrimaryHeatingUnitAFUE);
            vm.TemperatureControleclass = CheckforNull(result.EffectOfTemperatureRegulatorClass);
            vm.SupBoilerAnnualEfficiency = CheckforNull(result.SecondaryBoilerAFUE);
            vm.SupBoilerTotal = CheckforNull(result.EffectOfSecondaryBoiler);

            vm.SolarM2 = CheckforNull(result.SolarCollectorArea);
            vm.SolarM3 = CheckforNull(result.ContainerVolume);
            vm.SolarEfficiency = CheckforNull(result.SolarCollectorEffectiveness);//
            vm.SolarClass = CheckforNull(result.ContainerClassification);//
            vm.SolarTotal = CheckforNull(result.SolarHeatContribution);

            vm.PackagedAnnualEfficiencyEEILabel = SelectValue(result.EEICharacters);
            vm.ResultOne = CheckforNull(result.PackagedSolutionAtColdTemperaturesAFUE);
        }

        private string CheckforNull(float value)
        {
            if (value == 0)
            {
                return "";
            }
            return Math.Round(value, 2).ToString();
            
        }

        private void CalPageOne(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageOne = (true) ? "Visible" : "Collapsed";
            vm.ResultTwo = CheckforNull(result.PackagedSolutionAtWarmTemperaturesAFUE);
            vm.PackagedAnnualEfficiencyAverageClima = "N/A";
        }
        private void CalPageTwo(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageTwo = (true) ? "Visible" : "Collapsed";
            vm.SupHeatingUnitAnnualEfficiency = CheckforNull(result.EffectOfSecondaryHeatPump);
            vm.SupHeatingUnitTotal = CheckforNull(result.SecondaryHeatPumpAFUE);
            vm.SolarContributionAndSupHeatingUnitTotal = CheckforNull(result.AdjustedContribution);
            vm.PackagedAnnualEfficiencyRoomHeating = CheckforNull(result.EEI);
        }
        private void CalPageThree(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.Pagethree = (true) ? "Visible" : "Collapsed";
            vm.PackagedAnnualEfficiencyRoomHeating = CheckforNull(result.EEI);
        }
        private void CalPageFour(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageFour = (true) ? "Visible" : "Collapsed";
            vm.PackagedAnnualEfficiencyAverageClima = "N/A";
        }
        private void CalPageFive(PdfCalculationViewModel vm, EEICalculationResult result)
        {
            vm.PageFive = (true) ? "Visible" : "Collapsed";
            vm.UseProfile = "N/A";
            vm.AnnualWaterheatingEfficiency = "N/A";
        }

        private int SelectValue(string label)
        {
            int value;
            switch (label)
            {
                case "A+++": value = 10; break;
                case "A++": value = 9; break;
                case "A+": value = 8; break;
                case "A": value = 7; break;
                case "B": value = 6; break;
                case "C": value = 5; break;
                case "D": value = 4; break;
                case "E": value = 3; break;
                case "F": value = 2; break;
                case "G": value = 1; break;
                default:  value = 0; break;
            }
            return value;
        }

        private void SetUpLabelTwo(PackagedSolution packaged, List<EEICalculationResult> result, PdfLabelExportViewModel vm)
        {
            ArrowData data;
            vm.CompNameText = "Hjøring VVS";
            vm.LabelOne = (false) ? "Visible" : "Collapsed";
            vm.LabelTwo = (true) ? "Visible" : "Collapsed";

            vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            vm.TapValue = result[1].WaterHeatingUseProfile.ToString();

            data = SelectArrowValue(result[0].EEICharacters);
            vm.TabelOneArrow = data.location;
            vm.TabelOneArrowLetter = data.Letter;
            vm.TabelOneArrowPlus = data.Plus;

            data = SelectArrowValue(result[1].EEICharacters);
            vm.TabelTwoArrow = data.location;
            vm.TabelTwoArrowLetter = data.Letter;
            vm.TabelTwoArrowPlus = data.Plus;

            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[0].PrimaryHeatingUnitAFUE)[0]);
            vm.WaterHeatingModeLetter = data.Letter;
            vm.WaterHeatingModePlus = data.Plus;

            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result[1].PrimaryHeatingUnitAFUE)[0]);
            vm.AnnualEfficiencyLetter = data.Letter;
            vm.AnnualEfficiencyPlus = data.Plus;
        }
        private void SetUpLabelOne(PackagedSolution packaged, EEICalculationResult result, PdfLabelExportViewModel vm)
        {
            ArrowData data;
            vm.CompNameText = "Hjøring VVS";
            vm.LabelOne = (true) ? "Visible" : "Collapsed";
            vm.LabelTwo = (false) ? "Visible" : "Collapsed";

            vm.SolarIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.SolarPanel)) ? "Visible" : "Hidden";
            vm.WatertankIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.Container)) ? "Visible" : "Hidden";
            vm.TempControleIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.TemperatureController)) ? "Visible" : "Hidden";
            vm.HeaterIncluded = (packaged.Appliances.Any(item => item.Type == ApplianceTypes.CHP)) ? "Visible" : "Hidden";//Skal kigges på

            data = SelectArrowValue(result.EEICharacters);
            vm.LabelTwoTabeOneArrow = data.location;
            vm.LabelTwoTabeOneArrowLetter = data.Letter;
            vm.LabelTwoTabeOneArrowPlus = data.Plus;
            data = SelectArrowValue(EEICharLabelChooser.EEIChar(ApplianceTypes.Boiler, result.PrimaryHeatingUnitAFUE)[0]);
            vm.AnnualEfficiencyLetter = data.Letter;
            vm.AnnualEfficiencyPlus = data.Plus;
        }
        private ArrowData SelectArrowValue(string label)
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
                default: letterOnArrow = "Error"; arrowPlace = 10; break;
            }
            ArrowData ad = new ArrowData();
            ad.location = arrowPlace;
            ad.Letter = letterOnArrow;
            ad.Plus = PlusOnArrow;
            return ad;
        }
    }
}
