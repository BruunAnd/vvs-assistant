using MahApps.Metro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(App.SolutionPage);
        }

        private void addComponent_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.newFlyout.IsOpen = true;
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(App.ExistingSolutionsPage);
        }

        private void alternativePrototype_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(App.AlternativePage);
        }
    }
}