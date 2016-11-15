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
using MahApps.Metro.Controls.Dialogs;

namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for OfferPage.xaml
    /// </summary>
    public partial class OfferPage : Page
    {
        private BaseMetroDialog _printDialog;

        public Offer Offer { get; set; }

        public OfferPage()
        {
            InitializeComponent();
            InitializePage();
            Offer.PropertyChanged += UpdateSidebar;
            // Bound datacontext to this page, allowing the sidebar to retrieve information from the Offer through bindings.
            this.DataContext = this;
        }

        private void UpdateSidebar(object sender, PropertyChangedEventArgs e)
        {
            TotalSalesPrice.Content = Offer.TotalSalesPrice;
            TotalSalesPricePlusTax.Content = Offer.TotalSalesPricePlusTax;
            TotalCostPrice.Content = Offer.TotalCostPrice;
            TotalContributionMargin.Content = Offer.TotalContributionMargin;

            AppliancesSalesPrice.Content = Offer.Solution.Appliances.TotalSalesPrice();
            AppliancesCostPrice.Content = Offer.Solution.Appliances.TotalCostPrice();
            AppliancesContributionMargin.Content = Offer.Solution.Appliances.TotalContributionMargin();

            SalariesSalesPrice.Content = Offer.Salaries.TotalSalesPrice();
            SalariesCostPrice.Content = Offer.Salaries.TotalCostPrice();
            SalariesContributionMargin.Content = Offer.Salaries.TotalContributionMargin();

            MaterialsSalesPrice.Content = Offer.Materials.TotalSalesPrice();
            MaterialsCostPrice.Content = Offer.Materials.TotalCostPrice();
            MaterialsContributionMargin.Content = Offer.Materials.TotalContributionMargin();
        }
        
        private void InitializePage()
        {
            Offer = new Offer("Tilbud 3", App._packagedList[0]);
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

        private void PrintOffer(object sender, RoutedEventArgs e)
        {
            OfferLetter letter = new OfferLetter(Offer);
            _printDialog = (BaseMetroDialog) Resources["PrintOfferDialog"];
            _printDialog.DataContext = letter;

            App.MainWindow.ShowMetroDialogAsync(_printDialog);
        }

        private async void PrintOfferConfirm(object sender, RoutedEventArgs e)
        {
            await App.MainWindow.HideMetroDialogAsync(_printDialog);
            await App.MainWindow.ShowMessageAsync("grats", "u made an offer xD");
            _printDialog = null;
        }

        private void PrintOfferCancel(object sender, RoutedEventArgs e)
        {
            App.MainWindow.HideMetroDialogAsync(_printDialog);
            _printDialog = null;
        }
    }
}
