using System.Windows.Documents;

namespace VVSAssistant.Views
{
    /// <summary>
    /// Interaction logic for PdfOfferLayout.xaml
    /// </summary>
    public partial class OfferLayout
    {
        public FlowDocument FlowDocument;
        public OfferLayout()
        {
            InitializeComponent();
            FlowDocument = OfferDocument;
        }
    }
}
