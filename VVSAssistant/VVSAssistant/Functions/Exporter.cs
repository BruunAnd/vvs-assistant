using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.Views;

namespace VVSAssistant.Functions
{
    internal class Exporter
    {

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
            foreach (var variable in cal)
            {
                result.Add(variable.CalculateEEI(packaged));
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

            var fixedDoc = new FixedDocument();

            var v = new PdfLabelLayout();
            var vm = new PdfLabelExportViewModel();
            vm.Setup(packaged, result);

            fixedDoc.Pages.Add(CreatePageContent(v, vm));

            var calculationLayout = new PdfCalculationLayout();
            var calculationViewModel = new PdfCalculationViewModel();
            calculationViewModel.SetUp(calculationViewModel, result);

            fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));
            
            SaveXpsFile(path, fixedDoc);
        }

        private static PageContent CreatePageContent<T, TU>(T view, TU vm) where T : ContentControl
        {
            var pageContent = new PageContent();
            var fixedPage = new FixedPage
            {
                Height = 11.69*96,
                Width = 8.27*96
            };
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

        private static void SaveXpsFile(string path, FixedDocument fixedDocument)
        {

            if (File.Exists(path))
                File.Delete(path); // TODO: Notify that the user should close all PDFs before doing this
            var xpsd = new XpsDocument(path, FileAccess.ReadWrite);
            var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(fixedDocument);
            xpsd.Close();

        }
    }
}
