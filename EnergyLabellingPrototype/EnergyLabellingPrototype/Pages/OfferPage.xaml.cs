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
using System.Collections.ObjectModel;

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
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            NewOffer(sender, e);
        }

        private void SidebarRefresh(object sender, RoutedEventArgs e)
        {

        }



        private void CollapseControl(Control target)
        {
            target.Visibility = Visibility.Collapsed;
        }

        private void ExpandControl(Control target)
        {
            target.Visibility = Visibility.Visible;
        }
        
        private void FillDataGrid<T>(DataGrid dataGrid, ObservableCollection<T> fillData)
        {
            dataGrid.ItemsSource = fillData;
        }

        private void ClearDataGrid(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Refresh();
        }

        private void NewOffer(object sender, RoutedEventArgs e)
        {
            ClearDataGrid(dataGridComponents);
            if (dataGridComponents.Items.Count == 0)
            {
                CollapseControl(tabControl);
                FillDataGrid<Solution>(dataGridExistingSolutions, App._packagedList);
                ExpandControl(dataGridExistingSolutions);
            }
        }

        private void DoubleClickSolution(object sender, MouseButtonEventArgs e)
        {
            var UIel = Mouse.DirectlyOver as UIElement;
            if (UIel is Button)
            {
                return;
            }
            if (dataGridExistingSolutions.SelectedItem == null) return;
            var selectedSolution = dataGridExistingSolutions.SelectedItem as Solution;
            AddSolutionToOffer(selectedSolution);

        }

        private void AddSolutionToOffer(Solution src)
        {
            FillDataGrid(dataGridComponents, src.SolutionList);
            CollapseControl(dataGridExistingSolutions);
            ExpandControl(tabControl);
        }



        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
