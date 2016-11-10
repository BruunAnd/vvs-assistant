using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for OfferPage.xaml
    /// </summary>
    public partial class OfferPage : Page
    {
        public OfferPage()
        {
            InitializeComponent();

            //foreach(TabItem tab in tabControl.Items)
            //{
            //    tab.Content = OfferGrid("dataGrid" + tab.Name);
            //}
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        //private DataGrid OfferGrid(string binding)
        //{
        //    var columnHeaderDictionary = new Dictionary<string, string>()
        //        {
        //            {"description", "Beskrivelse"},
        //            {"amount", "Mængde"},
        //            {"unitPrice", "Enhedspris"},
        //            {"costPrice", "Kostpris"},
        //            {"salesUnitPrice", "Salgs EP"},
        //            {"salesPriceAbs", "Salgspris (Abs.)"},
        //            {"totalGrossMargin", "Samlet DB"}
        //        };

        //    var dg = new DataGrid();
        //    foreach (var entry in columnHeaderDictionary)
        //    {
        //        dg.Columns.Add(new DataGridTextColumn()
        //        {
        //            Header = entry.Value,
        //            Binding = new Binding(entry.Key)
        //        });
        //    }

        //    return dg;
        //}
    }
}
