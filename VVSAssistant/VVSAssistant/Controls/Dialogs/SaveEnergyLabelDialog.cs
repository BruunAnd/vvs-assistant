using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.Views;
using Size = System.Windows.Size;

namespace VVSAssistant.Controls.Dialogs
{
    public class SaveEnergyLabelDialog
    {
        private DocumentPaginator _fixedDocument;

        private PackagedSolution _packagedSolution;

        public PackagedSolution PackagedSolution
        {
            get { return _packagedSolution; }
            set
            {
                if (value == null) return;

                if (value.EnergyLabel.Count <= 0)
                    return;

                var fixedDoc = new FixedDocument();

                var v = new LabelLayout();
                var vm = new LabelExportViewModel(value);

                fixedDoc.Pages.Add(CreatePageContent(v, vm));

                var calculationLayout = new CalculationLayout();
                var calculationViewModel = new CalculationViewModel(value.EnergyLabel);
                calculationViewModel.Setup();

                fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));

                if (value.EnergyLabel.Count > 1)
                {
                    calculationLayout = new CalculationLayout();
                    calculationViewModel = new CalculationViewModel(value.EnergyLabel);
                    calculationViewModel.SetupSpecialPage();
                    fixedDoc.Pages.Add(CreatePageContent(calculationLayout, calculationViewModel));
                }

                IDocumentPaginatorSource dps = fixedDoc;
                _fixedDocument = dps.DocumentPaginator;

                _packagedSolution = value;
            }
        }

        public SaveEnergyLabelDialog(PackagedSolution packagedSolution)
        {
            PackagedSolution = packagedSolution;
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

        public void RunDialog()
        {
            if (PackagedSolution == null) return;

            var dlg = new SaveFileDialog()
            {
                Filter = "XPS-filer (.xps)|*.xps",
                FileName = PackagedSolution.Name,
                DefaultExt = ".xps"
            };

            var result = dlg.ShowDialog();

            if (result == false) return;

            if (File.Exists(dlg.FileName)) File.Delete(dlg.FileName);

            var xpsd = new XpsDocument(dlg.FileName, FileAccess.ReadWrite);
            var xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
            xw.Write(_fixedDocument);
            xpsd.Close();
        }
    }
}
