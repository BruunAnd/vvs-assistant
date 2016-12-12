using System.Windows.Documents;

namespace VVSAssistant.Views
{
    /// <summary>
    /// Interaction logic for PdfOfferLayout.xaml
    /// </summary>
    public partial class CompanyFooter
    {
        public FlowDocument FlowDocument;
        public CompanyFooter()
        {
            InitializeComponent();
            FlowDocument = FooterContent;
        }
    }
}
