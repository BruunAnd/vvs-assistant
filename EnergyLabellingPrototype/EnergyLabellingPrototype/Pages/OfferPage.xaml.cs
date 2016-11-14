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
using System.Diagnostics;
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
            InitializePage();
            // Bound datacontext to this page, allowing the sidebar to retrieve information from the Offer through bindings.
            this.DataContext = this;
        }

        private void InitializePage()
        {
            Offer = new Offer();
            dataGridExistingSolutions.ItemsSource = App._packagedList;
            dataGridSalary.ItemsSource = Offer.Salaries;
            dataGridMaterials.ItemsSource = Offer.Materials;
            ShowExistingSolutionsGrid();
        }

        private void ShowExistingSolutionsGrid()
        {
            CollapseControl(tabControl);
            ExpandControl(dataGridExistingSolutions);
        }

        private void ShowOfferGrid()
        {
            CollapseControl(dataGridExistingSolutions);
            ExpandControl(tabControl);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void NewOffer(object sender, RoutedEventArgs e)
        {
            Offer.Clear();
            ShowExistingSolutionsGrid();
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
            ShowOfferGrid();
        }
        
        private void CollapseControl(Control target)
        {
            target.Visibility = Visibility.Collapsed;
        }

        private void ExpandControl(Control target)
        {
            target.Visibility = Visibility.Visible;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
