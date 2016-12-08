using System;
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
            public static string Name => 
                new AppSettingsReader().GetValue("dbFileName", typeof(string)).ToString();

            public static bool Exists => Location != null;

            private static string Location => 
                Directory.GetFiles(AssemblyDirectory, Name).FirstOrDefault();

            public static bool Export(string targetPath)
            {
                // Path is invalid
                if (!Exists) return false;
                if (File.Exists(targetPath)) File.Delete(targetPath);
                using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(Location, Name);
                }

                return true;
            }

            public static bool Import(string src)
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                using (var archive = ZipFile.OpenRead(src))
                {
                    if (archive.Entries.FirstOrDefault(x => x.Name == Name) == null)
                    {
                        return false;
                    }
                }
                if (Exists) File.Delete(Location);
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

        public static class Offer
        {
            public static void Export(Models.Offer offer)
            {
                //var path = $"Offer.xps";
                var fixedDoc = new FixedDocument();

                //PageOne
                var vmOffer = new OfferExportViewModel(offer);
                var pageOne = new OfferLayout();
                vmOffer.PageOne = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageOne, vmOffer));

                //PageTwo
                vmOffer = new OfferExportViewModel(offer);
                var pageTwo = new OfferLayout();
                vmOffer.PageTwo = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageTwo, vmOffer));

                //pageThree
                vmOffer = new OfferExportViewModel(offer);
                var pageThree = new OfferLayout();
                vmOffer.PageThree = "Visible";
                fixedDoc.Pages.Add(CreatePageContent(pageThree, vmOffer));

                RunSaveDialog(fixedDoc, offer.OfferInformation.Title);
            }

            
        }

        public static class EnergyLabel
        {
            public static void ExportEnergyLabel(PackagedSolution packaged)
            {
                var fixedDoc = new FixedDocument();

                var v = new LabelLayout();
                var vm = new LabelExportViewModel(packaged);

                fixedDoc.Pages.Add(CreatePageContent(v, vm));

                var calculationLayout = new CalculationLayout();
                var calculationViewModel = new CalculationViewModel(packaged.EnergyLabel);
                calculationViewModel.Setup();

                fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));

                if (packaged.EnergyLabel.Count > 1)
                {
                    calculationLayout = new CalculationLayout();
                    calculationViewModel = new CalculationViewModel(packaged.EnergyLabel);
                    calculationViewModel.SetupSpecialPage();
                    fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));
                }

                RunSaveDialog(fixedDoc, packaged.Name);
            }
        }
    }
}
