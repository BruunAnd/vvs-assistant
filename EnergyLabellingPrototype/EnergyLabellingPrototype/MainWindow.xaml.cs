using EnergyLabellingPrototype.Pages;
using MahApps.Metro.Controls;
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

namespace EnergyLabellingPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            App.MainWindow = this;
        }

        private void saveComponent_Click(object sender, RoutedEventArgs e)
        {
            App._componentList.Add(new Component(TextBox_Name.Text, TextBox_Info.Text, ComboBox_Type.Text, 85000));
            newFlyout.IsOpen = false;
            TextBox_Name.Text = "";
            TextBox_Info.Text = "";
            ComboBox_Type.Text = "";
        }

        private void saveEditComponent_Click(object sender, RoutedEventArgs e)
        {
            //Component c = new Component(TextBox_Name_Edit.Text, TextBox_Info_Edit.Text, ComboBox_Type_Edit.Text);
            App._componentList[int.Parse(Label_Index_Edit.Text)].Name = TextBox_Name_Edit.Text;
            App._componentList[int.Parse(Label_Index_Edit.Text)].Description = TextBox_Info_Edit.Text;
            App._componentList[int.Parse(Label_Index_Edit.Text)].Type = ComboBox_Type_Edit.Text;
            
            Edit.IsOpen = false;
        }

        public void Info_in_fly(Component component, int index)
        {
            editToggleSwitch.IsChecked = false;
            TextBox_Info_Edit.Text = component.Description;
            Label_Index_Edit.Text = index.ToString();
            TextBox_Name_Edit.Text = component.Name;
            ComboBox_Type_Edit.Text = component.Type;
        }

        public void Solution_Info(Solution solution) 
        {
            dataGridinfo.ItemsSource = solution.SolutionList.Where((item) => item.Name != null);
        }

        private void ToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Name_Edit.IsEnabled = editToggleSwitch.IsChecked.Value;
            ComboBox_Type_Edit.IsEnabled = editToggleSwitch.IsChecked.Value;
            TextBox_Info_Edit.IsEnabled = editToggleSwitch.IsChecked.Value;
            saveComponent_Edit.Visibility = editToggleSwitch.IsChecked.Value ? Visibility.Visible : Visibility.Hidden;
        }      
    }
}
