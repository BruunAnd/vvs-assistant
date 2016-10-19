using MahApps.Metro.Controls;
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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;


namespace EnergyLabellingPrototype.Pages
{
    /// <summary>
    /// Interaction logic for AlternativePage.xaml
    /// </summary>
    public partial class AlternativePage : Page
    {
        private int Pack_Id;
        private string Pack;
        private ObservableCollection<Component> _packagedComponents = new ObservableCollection<Component>();
        public AlternativePage()
        {
            InitializeComponent();
        }

        public AlternativePage(Solution solution, int id) : this()
        {
            Pack_Id = id;
            Pack = "";
            Add_Solution_Too_Shop(solution);
        }

        private void Add_Solution_Too_Shop(Solution solution)
        {
            foreach (var item in solution.SolutionList)
            {
                _packagedComponents.Add(item);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Page();
        }

        private void Update_Page()
        {
            RefreshDynamicItemSources();
            dataGridPackage.ItemsSource = _packagedComponents;
        }

        private void RefreshDynamicItemSources()
        {
            var filterText = textBoxSearch.Text.ToLower();

            // Update component list
            dataGridComponents.ItemsSource = App._componentList.Where(c => c.FilterMatch(filterText));
            dataGridComponents.Items.Refresh();

            // Update solution list
            dataGridSolutions.ItemsSource = App._packagedList.Where(s => s.FilterMatch(filterText));
            dataGridSolutions.Items.Refresh();
        }

       

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Component;
            Component c = new Component(item.Name, item.Description, item.Type);
            App.MainWindow.Edit.IsOpen = true;
            int count = 0;
            foreach (Component VARIABLE in App._componentList)
            {
                if (VARIABLE.Counter.Equals(item.Counter))
                {
                    break;
                }
                count++;
            }
            App.MainWindow.Info_in_fly(c, count);
        }

        private void AddToPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Component;
            Component c = new Component(item.Name, item.Description, item.Type);
            if (c.Type.Equals("Kedel"))
            {

                MessageBoxResult result = MessageBox.Show("Ja = normal kedel \nNej = supplerende kedel", "Valg", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        c.Type = "kedel";
                        Add_Component_Too_Shop(c);
                        break;
                    case MessageBoxResult.No:
                        c.Type = "Supplerende kedel";
                        Add_Component_Too_Shop(c);
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                Add_Component_Too_Shop(c);
            }

        }

        private void Add_Component_Too_Shop(Component item)
        {
            _packagedComponents.Add(item);
            dataGridPackage.Items.Refresh();
        }
        private void AddSolToPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Solution;
            Pack_Id = item.Counter;
            Pack = "";
            Add_Solution_Too_Shop(item);
            dataGridPackage.Items.Refresh();
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _packagedComponents.Remove((sender as Button).DataContext as Component);
            dataGridPackage.Items.Refresh();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if (_packagedComponents.Count > 0)
            {
                if (Pack != null)
                {
                    foreach (var test in App._packagedList)
                    {
                        if (test.Counter.Equals(Pack_Id))
                        {
                            App._packagedList[count].SolutionList.Clear();
                            foreach (Component item in _packagedComponents)
                            {
                                App._packagedList[count].SolutionList.Add(item);
                            }
                            break;
                        }
                        count++;
                    }
                    _packagedComponents.Clear();
                    Update_Page();
                    dataGridPackage.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Ingen pakke at gemme til");
                }

                Pack = null;
            }
            else
            {
                MessageBox.Show("Listen er tom!");
            }

        }
        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as Solution;
            Solution c = new Solution(item.Name, item.SolutionList);
            App.MainWindow.infosolution.IsOpen = true;
            App.MainWindow.Solution_Info(c);
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDynamicItemSources();
        }

        private void RemovePackButton_Click(object sender, RoutedEventArgs e)
        {
            Pack = null;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog)Resources["CustomCloseDialogTest"];
            //await App.MainWindow.ShowMetroDialog(dialog);
        }



        private void addComponent_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.newFlyout.IsOpen = true;
        }

        private void showPackages_Click(object sender, RoutedEventArgs e)
        {
            dataGridSolutions.Visibility = Visibility.Visible;
            showComponents.Visibility = Visibility.Visible;
            dataGridComponents.Visibility = Visibility.Hidden;
            showPackages.Visibility = Visibility.Hidden;
        }

        private void showComponents_Click(object sender, RoutedEventArgs e)
        {
            dataGridComponents.Visibility = Visibility.Visible;
            showPackages.Visibility = Visibility.Visible;
            dataGridSolutions.Visibility = Visibility.Hidden;
            showComponents.Visibility = Visibility.Hidden;
        }

        private void clearPackageConstructor_Click(object sender, RoutedEventArgs e)
        {
            if(_packagedComponents.Any())
            {
                // List of components in the temporary package is not empty, warn the user.
                MessageBoxResult result = MessageBox.Show("Du har tilføjet komponenter til pakkeløsningen, er du sikker på at du vil rydde listen for at påbegynde en ny beregning?", "Bekræftelse", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _packagedComponents.Clear();
                    dataGridPackage.Items.Refresh();
                    Pack = null;
                }
            }
            else
            {
                // List of components in the temporary package is empty, inform the user.
                MessageBox.Show("Du har ikke tilføjet komponenter til pakkeløsningen, tryk på det sorte '+' til højre for et komponent i listen for at tilføje det til pakkeløsningen.");
            }
        }

        private void savePackageConstruction_Click(object sender, RoutedEventArgs e)
        {
            if (_packagedComponents.Any())
            {
                string name = constructedPackageName.Text;

                if(string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Du har ikke angivet et navn for pakkeløsningen, skriv et navn i feltet ovenfor og prøv igen.");
                    constructedPackageName.Focus();
                } 
                else
                {
                    Solution s = new Solution("Pakke", _packagedComponents);
                    App._packagedList.Add(s);
                    s.Name = name;
                    Update_Page();
                    dataGridPackage.Items.Refresh();
                    MessageBox.Show("Din pakke er blevet gemt, du kan finde den i listen over pakkeløsninger ved at trykke på 'Pakkeløsninger' knappen ved siden af søgefeltet.");
                }
            }
            else
            {
                // List of components in the temporary package is empty, inform the user.
                MessageBox.Show("Du har ikke tilføjet komponenter til pakkeløsningen, tryk på det sorte '+' til højre for et komponent i listen for at tilføje det til pakkeløsningen.");
            }
        }
    }
}
