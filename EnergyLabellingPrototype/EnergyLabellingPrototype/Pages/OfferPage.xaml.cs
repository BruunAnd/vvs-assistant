using System;
using EnergyLabellingPrototype.Models;
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
    public partial class OfferPage : Page
    {
        public Offer Offer { get; set; }

        public OfferPage()
        {
            InitializeComponent();
            this.DataContext = this;
            InitializePage();
        }

        private void InitializePage()
        {
            dataGridExistingSolutions.ItemsSource = App._packagedList;
            Offer = new Offer();
            dataGridSalary.ItemsSource = Offer.Salaries;
            dataGridMaterials.ItemsSource = Offer.Materials;
            CollapseControl(tabControl);
            ExpandControl(dataGridExistingSolutions);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void NewOffer(object sender, RoutedEventArgs e)
        {
            Offer.Salaries.Clear();
            Offer.Materials.Clear();
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
            dataGridAppliances.ItemsSource = Offer.Solution.Appliances;
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

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        
        
    }
}
