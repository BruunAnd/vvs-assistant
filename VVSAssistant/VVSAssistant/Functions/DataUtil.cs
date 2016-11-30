using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.Views;

namespace VVSAssistant.Functions
{
    public static class DataUtil
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static class Database
        {
            public static string Name()
            {
                return new AppSettingsReader().GetValue("dbFileName", typeof(string)).ToString();
            }

            public static bool Exists()
            {
                return Location() != null;
            }

            private static string Location()
            {
                return Directory.GetFiles(AssemblyDirectory, Name()).FirstOrDefault();
            }

            public static bool Export(string targetPath)
            {
                // Path is invalid
                if (!Exists()) return false;
                if (File.Exists(targetPath)) File.Delete(targetPath);
                using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(Location(), Name());
                }

                return true;
            }

            public static bool Import(string src)
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var archive = ZipFile.OpenRead(src))
                {
                    if (archive.Entries.FirstOrDefault(x => x.Name == Name()) == null)
                    {
                        return false;
                    }
                }
                if (Exists()) File.Delete(Location());
                ZipFile.ExtractToDirectory(src, assemblyLocation);
                return true;
            }
        }

        private static PageContent CreatePageContent<T, TU>(T view, TU vm) where T : ContentControl
        {
            var pageContent = new PageContent();
            var fixedPage = new FixedPage
            {
                Height = 11.69 * 96,
                Width = 8.27 * 96
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
        
        public static class PdfOffer
        {
            public static void Export(Offer offer)
            {
                //var path = $"PdfOffer.xps";
                var fixedDoc = new FixedDocument();
                var vmOffer = new PdfOfferExportViewModel();
                vmOffer.SetUp(offer);

                //PageOne
                var pageOne = new PdfOfferLayout();
                vmOffer.PageOne = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageOne, vmOffer));

                //PageTwo
                vmOffer = new PdfOfferExportViewModel();
                var pageTwo = new PdfOfferLayout();
                vmOffer.PageTwo = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageTwo, vmOffer));

                //pageThree
                vmOffer = new PdfOfferExportViewModel();
                var pageThree = new PdfOfferLayout();
                vmOffer.PageThree = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageThree, vmOffer));

                RunSaveDialog(fixedDoc, offer.OfferInformation.Title);
            }

            private static void RunSaveDialog(FixedDocument fixedDocument, string offerTitle)
            {
                var dlg = new SaveFileDialog()
                {
                    Filter = "XPS-filer (.xps)|*.xps",
                    FileName = offerTitle,
                    DefaultExt = ".xps"
                };

                var result = dlg.ShowDialog();
                if (result == false) return;
                if (File.Exists(dlg.FileName)) File.Delete(dlg.FileName);

                var xpsd = new XpsDocument(dlg.FileName, FileAccess.ReadWrite);
                var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(fixedDocument);
                xpsd.Close();
            }
        }

        public static class PdfEnergyLabel
        {
            public static void ExportEnergyLabel(PackagedSolution packaged)
            {

                #region Debug

                var cm = new CalculationManager();
                var cal = cm.SelectCalculationStrategy(packaged);

                if (cal == null)
                {
                    return;
                }

                var result = cal.Select(variable => variable.CalculateEEI(packaged)).ToList();

                #endregion
                #region Debug
                Console.WriteLine("Print Label pdf == Done");
                Console.WriteLine("Count " + result.Count);
                //Console.WriteLine("Index 0 " + result[0].WaterHeatingUseProfile);
                //Console.WriteLine(result[0].PrimaryHeatingUnitAFUE);
                Console.WriteLine("Main for index 0 " + result[0].EEICharacters);
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
                calculationViewModel.Setup(result);

                fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));

                if (result.Count > 1)
                {
                    calculationLayout = new PdfCalculationLayout();
                    calculationViewModel = new PdfCalculationViewModel();
                    calculationViewModel.SetupSpecialPage(result);
                    fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));
                }



                RunSaveDialog(fixedDoc, packaged.Name);
            }

            private static void RunSaveDialog(FixedDocument fixedDocument, string packagedSolutionName)
            {
                var dlg = new SaveFileDialog()
                {
                    Filter = "XPS-filer (.xps)|*.xps",
                    FileName = packagedSolutionName,
                    DefaultExt = ".xps"
                };

                var result = dlg.ShowDialog();
                if (result == false) return;
                if (File.Exists(dlg.FileName)) File.Delete(dlg.FileName);

                var xpsd = new XpsDocument(dlg.FileName, FileAccess.ReadWrite);
                var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(fixedDocument);
                xpsd.Close();
            }
        }
    }
}
