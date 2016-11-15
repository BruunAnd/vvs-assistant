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
    /// Interaction logic for ExistingOffersPage.xaml
    /// </summary>
    public partial class ExistingOffersPage : Page
    {
        public ExistingOffersPage()
        {
            InitializeComponent();
            dataGridExistingOffers.ItemsSource = App._offerList;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
