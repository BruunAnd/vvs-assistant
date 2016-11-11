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
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for OfferPage.xaml
    /// </summary>
    public partial class OfferPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Offer offer;
        public Offer Offer
        {
            get { return offer; }
            set { offer = value; NotifyPropertyChanged(); }
        }
        
        public OfferPage()
        {
            InitializeComponent();
            InitializePage();
        }

        private void SalaryTabLoaded(object sender, RoutedEventArgs e)
        {
            BindDataGrid(dataGridSalary, Offer.SalaryList);
        }

        private void MaterialTabLoaded(object sender, RoutedEventArgs e)
        {
            BindDataGrid(dataGridMaterials, Offer.MaterialList);
        }

        private void InitializePage()
        {
            Offer = new Offer();

            CollapseControl(tabControl);
            ExpandControl(dataGridExistingSolutions);

            BindDataGrid<Solution>(dataGridExistingSolutions, App._packagedList);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void NewOffer(object sender, RoutedEventArgs e)
        {
            ClearDataGrid(dataGridComponents);
            ClearDataGrid(dataGridSalary);
            ClearDataGrid(dataGridMaterials);
            CollapseControl(tabControl);
            ExpandControl(dataGridExistingSolutions);
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
            Offer.Solution = src;
            BindDataGrid(dataGridComponents, Offer.Solution.Components);
            CollapseControl(dataGridExistingSolutions);
            ExpandControl(tabControl);
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
        
        private void BindDataGrid<T>(DataGrid dataGrid, ObservableCollection<T> fillData)
        {
            dataGrid.ItemsSource = fillData;
        }

        private void ClearDataGrid(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Refresh();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dataGridComponentsCurrentCellChanged(object sender, EventArgs e)
        {
            dataGridComponents.Items.Refresh();
        }
        
    }
}
