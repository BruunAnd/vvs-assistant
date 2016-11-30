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
        public void ExportOffer(Offer offer)
        {
            var path = $"PdfOffer{offer.OfferInformation.Title}.xps"; //TODO: Throw an exception when the name already exists, ask for another name
            FixedDocument fixedDoc = new FixedDocument();
            PdfOfferExportViewModel vmOffer = new PdfOfferExportViewModel();

            //PageOne
            PdfOfferLayout pageOne = new PdfOfferLayout();
            vmOffer.PageOne ="Visible";
            fixedDoc.Pages.Add(createPageContent(pageOne, vmOffer));

            //PageTwo
            PdfOfferLayout pageTwo = new PdfOfferLayout();
            vmOffer.PageTwo = "Visible";
            fixedDoc.Pages.Add(createPageContent(pageTwo, vmOffer));

            //pageThree
            PdfOfferLayout pageThree = new PdfOfferLayout();
            vmOffer.PageThree = "Visible";
            fixedDoc.Pages.Add(createPageContent(pageThree, vmOffer));

            SaveXpsFile(path, fixedDoc);
        }


        public void ExportEnergyLabel(PackagedSolution packaged)
        {

#region Debug
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
#endregion
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

            var path = $"PdfOffer{packaged.Name}.xps";

            FixedDocument fixedDoc = new FixedDocument();

            PdfLabelLayout v = new PdfLabelLayout();
            PdfLabelExportViewModel vm = new PdfLabelExportViewModel();
            vm.Setup(packaged, result);

            fixedDoc.Pages.Add(createPageContent(v, vm));

            PdfCalculationLayout calculationLayout = new PdfCalculationLayout();
            PdfCalculationViewModel calculationViewModel = new PdfCalculationViewModel();
            calculationViewModel.SetUp(calculationViewModel, result);

            fixedDoc.Pages.Add(createPageContent(calculationLayout, calculationViewModel));
            
            SaveXpsFile(path, fixedDoc);
        }

        private PageContent createPageContent<T, U>(T view, U vm) where T : ContentControl
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
    }
}
