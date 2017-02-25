using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MS.Internal.AppModel;
using MS.Win32;
using VVSAssistant.Models;
using VVSAssistant.Views;
using VVSAssistant.ViewModels;
using FontFamily = System.Windows.Media.FontFamily;
using System.Windows.Documents;
using System.IO;
using System.Windows.Xps.Packaging;

namespace VVSAssistant.Controls.Dialogs
{
    public class SaveOfferDialog
    {
        private DocumentPaginator _fixedDocument;

        private Offer _offer;
        public Offer Offer {
            get
            {
                return _offer;
            }
            set
            {
                if (value == null) return;

                var layout = new OfferLayout
                {
                    DataContext = new OfferExportViewModel(value),
                    FontFamily = new FontFamily("Calibri")
                };

                IDocumentPaginatorSource dps = layout;
                _fixedDocument = dps.DocumentPaginator;

                _offer = value;
            }
        }



        public void RunDialog()
        {
            if (Offer == null) return;

            var dlg = new SaveFileDialog()
            {
                Filter = "XPS-filer (.xps)|*.xps",
                FileName = Offer.OfferInformation.Title,
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
        
        public SaveOfferDialog(Offer offer) 
        {
            Offer = offer;
        }
       
    }
}
