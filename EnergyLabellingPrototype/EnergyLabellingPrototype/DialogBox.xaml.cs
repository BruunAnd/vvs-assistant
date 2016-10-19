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
using System.Windows.Shapes;

namespace EnergyLabellingPrototype
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox
    {
        public DialogBox()
        {
            InitializeComponent();
        }

        private Solution s;
        public DialogBox(Solution solution) : this()
        {
            s = solution;
        }

        private void Save_Clik(object sender, RoutedEventArgs e)
        {
            if (TextBox_Name.Text.Length >0)
            {
                s.Name = TextBox_Name.Text;
                this.Close();
            }
            
        }
    }
}
