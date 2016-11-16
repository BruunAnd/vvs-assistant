using MahApps.Metro.Controls;
using EnergyLabellingPrototype.Models;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
using Button = System.Windows.Controls.Button;

namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for SolutionPage.xaml
    /// </summary>
    public partial class SolutionPage : Page
    {
        private ObservableCollection<Appliance> _packagedComponents = new ObservableCollection<Appliance>();
        private ObservableCollection<Appliance> _components = App._componentList;

        public SolutionPage()
        {
            InitializeComponent();
            InitializePage();
        }

        public SolutionPage(Solution solution, int id) : this()
        {
            LoadPackageSolution(solution);
        }

        public void InitializePage()
        {
            
            // Bind collection of components to datagrid of components.
            dataGridComponents.ItemsSource = _components;

            // Bind collection of components in the current package solution to the datagrid of components in the package solution.
            dataGridPackage.ItemsSource = _packagedComponents;

            // Set DataContext to this, allowing binders to access properties directly from here.
            this.DataContext = this;
        }

        private void LoadPackageSolution(Solution solution)
        {
            // Add appliances to the current package solution in construction.
            foreach (Appliance item in solution.Appliances) _packagedComponents.Add(item);
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = textBoxSearch.Text.ToLower();
            dataGridComponents.ItemsSource = _components.Where(x => x.FilterMatch(filterText));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Page();
        }

        private void Update_Page()
        {
            RefreshItemSources();
        }

        private void RefreshItemSources()
        {
            dataGridComponents.Items.Refresh();
            dataGridPackage.Items.Refresh();
        }

        #region Context Menu
        private void AddToSolutionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (Appliance item in dataGridComponents.SelectedItems) AddToSolution(item);
        }

        private void EditApplianceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // EEEEW FIX CODE PLEASE.
            var item = dataGridComponents.SelectedItem as Appliance;
            Appliance c = new Appliance(item.Name, item.Description, item.Type, item.SalesPrice);
            App.MainWindow.Edit.IsOpen = true;
            int count = 0;
            foreach (Appliance VARIABLE in App._componentList)
            {
                if (VARIABLE.Counter.Equals(item.Counter))
                {
                    break;
                }
                count++;
            }
            App.MainWindow.Info_in_fly(c, count);
        }

        private void DeleteApplianceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // NOT A HACK JOB. 
            // In order to remove multiple items from the datagrid, one must not delete the items whilst itterating over the selected items, as doing so would disturb the lookup. 
            if(dataGridComponents.SelectedItems.Count > 1)
            {
                List<Appliance> selectedAppliances = new List<Appliance>();
                foreach (var item in dataGridComponents.SelectedItems) selectedAppliances.Add(item as Appliance);
                foreach (var item in selectedAppliances) _components.Remove(item);
            }
            else
            {
                var item = dataGridComponents.SelectedItem as Appliance;
                
                _components.Remove(item);
            }   
        }
        #endregion


        private void AddToSolution(Appliance item)
        {
            _packagedComponents.Add(item);
        }
        
        
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _packagedComponents.Remove((sender as Button).DataContext as Appliance);
            dataGridPackage.Items.Refresh();
        }
        

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            _packagedComponents.Clear();
            dataGridPackage.Items.Refresh();
        }

        private  async void SaveNewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_packagedComponents.Count > 0)
            {
                Solution s = new Solution("Pakke", _packagedComponents);
                App._packagedList.Add(s);

                string name = await App.MainWindow.ShowInputAsync("Pakkeløsning", "Angiv et navn til den nye pakkeløsning:");
                if (string.IsNullOrEmpty(name))
                    return;
                s.Name = name;
                
                Update_Page();
                dataGridPackage.Items.Refresh();
            }
            else
            {
                await App.MainWindow.ShowMessageAsync("Fejl", "Du skal tilføje komponenter til pakkeløsningen før den kan gemmes");
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog) Resources["CustomCloseDialogTest"];
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_packagedComponents.Count > 0)
            {
                await App.MainWindow.ShowMessageAsync("Success", "Dit pakkelabel er blevet printet");
            }
            else
            {
                await App.MainWindow.ShowMessageAsync("Fejl", "Du skal tilføje komponenter til pakkeløsningen før den kan printes");
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog) this.Resources["AddBoilerDialog"];

            await App.MainWindow.HideMetroDialogAsync(dialog);
        }

        private void addComponent_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.newFlyout.IsOpen = true;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
