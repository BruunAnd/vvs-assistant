using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
    /// Interaction logic for ExistingSolutionsPage.xaml
    /// </summary>
    public partial class ExistingSolutionsPage : Page
    {

        public ExistingSolutionsPage()
        {
            InitializeComponent();
        }

        private void AddToCalButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Solution;

            NavigationService.Navigate(new SolutionPage(item, item.Counter));
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = App._packagedList.Where((item) => item.Name != null);
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Solution;
            Solution c = new Solution(item.Name, item.Components);
            App.MainWindow.infosolution.IsOpen = true;
            App.MainWindow.Solution_Info(c);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NavigationPage());
        }
    }
}
