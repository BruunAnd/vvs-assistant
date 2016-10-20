using EnergyLabellingPrototype.Pages;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace EnergyLabellingPrototype
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Page _existingSolutionsPage, _solutionPage, _alternativePage;

        public static MainWindow MainWindow;

        public static ObservableCollection<Solution> _packagedList = new ObservableCollection<Solution>();
        public static ObservableCollection<Component> _componentList = new ObservableCollection<Component>();


        public App()
        {
            ObservableCollection<Component> list = new ObservableCollection<Component>();
            list.Add(new Component("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel"));
                list.Add(new Component("Compress 6000 LW", "Varmepumpe fra Bosch", "Varmepumpe"));
                list.Add(new Component("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));

            ObservableCollection<Component> list2 = new ObservableCollection<Component>();
            list2.Add(new Component("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel"));
            list2.Add(new Component("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            list2.Add(new Component("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation"));
            list2.Add(new Component("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger"));
            list2.Add(new Component("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder"));
            list2.Add(new Component("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder"));

            _packagedList.Add(new Solution("Pakke",list));
            _packagedList.Add(new Solution("Pakke", list2));


                _componentList.Add(new Component("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel"));
            _componentList.Add(new Component("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe"));
            _componentList.Add(new Component("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe"));
            _componentList.Add(new Component("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation"));
            _componentList.Add(new Component("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder"));
            _componentList.Add(new Component("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder"));
            _componentList.Add(new Component("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger"));
            _componentList.Add(new Component("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel"));
            _componentList.Add(new Component("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe"));
            _componentList.Add(new Component("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe"));
            _componentList.Add(new Component("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation"));
            _componentList.Add(new Component("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder"));
            _componentList.Add(new Component("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder"));
            _componentList.Add(new Component("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger"));
            _componentList.Add(new Component("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel"));
            _componentList.Add(new Component("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel"));
            _componentList.Add(new Component("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe"));
            _componentList.Add(new Component("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe"));
            _componentList.Add(new Component("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring"));
            _componentList.Add(new Component("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation"));
            _componentList.Add(new Component("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder"));
            _componentList.Add(new Component("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder"));
            _componentList.Add(new Component("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger"));

        }

        public static Page ExistingSolutionsPage
        {
            get
            {
                return _existingSolutionsPage == null ? _existingSolutionsPage = new ExistingSolutionsPage() : _existingSolutionsPage;
            }
        }

        public static Page SolutionPage
        {
            get
            {
                return _solutionPage == null ? _solutionPage = new SolutionPage() : _solutionPage;
            }
        }

        public static Page AlternativePage
        {
            get
            {
                return _alternativePage == null ? _alternativePage = new AlternativePage() : _alternativePage;
            }
        }
    }

}
