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
            // Initialize component and page.
            InitializeComponent();
            InitializePage();

            // Bind the property changed event handler to update sidebar info.
            Offer.PropertyChanged += UpdateSidebar;

            // Set DataContext to this, allowing binders to access properties directly from here.
            this.DataContext = this;
        }

        public OfferPage(Offer offer) : this()
        {
            Offer = offer;
            ShowOfferGrid();
        }

        public OfferPage(Solution solution) : this()
        {
            AddSolutionToOffer(solution);
        }

        private void InitializePage()
        {
            Offer = new Offer();

            // Bind data sources to the datagrids.
            dataGridExistingSolutions.ItemsSource = App._packagedList;
            dataGridSalary.ItemsSource = Offer.Salaries;
            dataGridMaterials.ItemsSource = Offer.Materials;
            dataGridAppliances.ItemsSource = Offer.Appliances;

            // Check wether appliances has been added to the current offer 
            // (eg. entering the create offer page by clicking an existing offer in the existing offers page).
            if(!Offer.Appliances.Any())
            {
                ShowExistingSolutionsGrid();
            }
        }

        private void UpdateItemSources()
        {
            dataGridSalary.ItemsSource = Offer.Salaries;
            dataGridMaterials.ItemsSource = Offer.Materials;
            dataGridAppliances.ItemsSource = Offer.Appliances;
        }

        private void UpdateSidebar(object sender, PropertyChangedEventArgs e)
        {
            // Bind properties from the offer to the various labels content, will update with property changes in the datagrids.
            TotalSalesPrice.Content = Offer.TotalSalesPrice;
            TotalSalesPricePlusTax.Content = Offer.TotalSalesPricePlusTax;
            TotalCostPrice.Content = Offer.TotalCostPrice;
            TotalContributionMargin.Content = Offer.TotalContributionMargin;

            AppliancesSalesPrice.Content = Offer.Appliances.TotalSalesPrice();
            AppliancesCostPrice.Content = Offer.Appliances.TotalCostPrice();
            AppliancesContributionMargin.Content = Offer.Appliances.TotalContributionMargin();

            SalariesSalesPrice.Content = Offer.Salaries.TotalSalesPrice();
            SalariesCostPrice.Content = Offer.Salaries.TotalCostPrice();
            SalariesContributionMargin.Content = Offer.Salaries.TotalContributionMargin();

            MaterialsSalesPrice.Content = Offer.Materials.TotalSalesPrice();
            MaterialsCostPrice.Content = Offer.Materials.TotalCostPrice();
            MaterialsContributionMargin.Content = Offer.Materials.TotalContributionMargin();
        }

        /// <summary>
        /// Shows the existing solutions grid and hides the offer grid (= tab control).
        /// </summary>
        private void ShowExistingSolutionsGrid()
        {
            CollapseControl(tabControl);

            // Refresh data bound to the existing solutions datagrid before showing.
            dataGridExistingSolutions.Items.Refresh();

            ExpandControl(dataGridExistingSolutions);
        }

        /// <summary>
        /// Shows the offer grid and hides the existing solutions grid.
        /// </summary>
        private void ShowOfferGrid()
        {
            CollapseControl(dataGridExistingSolutions);
            ExpandControl(tabControl);
        }

        #region Sidebar Buttons
        private void New(object sender, RoutedEventArgs e)
        {
            // Once a propper database sync has been setup for the save function, this should clear the data in the offer.
            ShowExistingSolutionsGrid();
        }

        private async void Save(object sender, RoutedEventArgs e)
        {
            // This is a referenced version, meaning if you make more than one save they will share the same referenced objects. Needs to be addressed with database.
            string name = await App.MainWindow.ShowInputAsync("Tilbud", "Angiv et navn for tilbuddet:");
            if (string.IsNullOrEmpty(name))
                return;
            Offer.Name = name;

            App._offerList.Add(Offer);
            await App.MainWindow.ShowMessageAsync("Tilbud gemt", "Dit tilbud er nu gemt under navnet " + name);
        }

        private void Print(object sender, RoutedEventArgs e)
        {
            _printDialog = (BaseMetroDialog)Resources["PrintOfferDialog"];
            _printDialog.DataContext = Offer;

            App.MainWindow.ShowMetroDialogAsync(_printDialog);
        }
        #endregion
        

        #region Print dialog buttons
        private async void PrintConfirm(object sender, RoutedEventArgs e)
        {
            await App.MainWindow.HideMetroDialogAsync(_printDialog);
            await App.MainWindow.ShowMessageAsync("grats", "u made an offer xD");
            _printDialog = null;
        }

        private void PrintCancel(object sender, RoutedEventArgs e)
        {
            App.MainWindow.HideMetroDialogAsync(_printDialog);
            _printDialog = null;
        }
        #endregion

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
            foreach(Appliance item in src.Appliances)
            {
                Offer.Appliances.Add(item);
            }

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
