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
            var col = new string[] {"Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"};
            foreach (var item in col)
                themes.Items.Add(item);
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

        private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(themes.SelectedValue.ToString()), ThemeManager.GetAppTheme("BaseLight"));
        }
    }
}