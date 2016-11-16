using EnergyLabellingPrototype.Pages;
using EnergyLabellingPrototype.Models;
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
        private static Page _existingSolutionsPage, _solutionPage, _offerPage, _existingOffersPage;

        public static new MainWindow MainWindow;

        public static ObservableCollection<Solution> _packagedList = new ObservableCollection<Solution>();
        public static ObservableCollection<Appliance> _componentList = new ObservableCollection<Appliance>();
        public static ObservableCollection<Offer> _offerList = new ObservableCollection<Offer>();

        public App()
        {
            ObservableCollection<Appliance> list = new ObservableCollection<Appliance>();
            list.Add(new Appliance("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel", 85000));
            list.Add(new Appliance("Compress 6000 LW", "Varmepumpe fra Bosch", "Varmepumpe", 85000));
            list.Add(new Appliance("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));

            ObservableCollection<Appliance> list2 = new ObservableCollection<Appliance>();
            list2.Add(new Appliance("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel", 85000));
            list2.Add(new Appliance("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            list2.Add(new Appliance("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation", 85000));
            list2.Add(new Appliance("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger", 85000));
            list2.Add(new Appliance("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder", 85000));
            list2.Add(new Appliance("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder", 85000));

            _packagedList.Add(new Solution("Pakke",list));
            _packagedList.Add(new Solution("Pakke", list2));
            

            _componentList.Add(new Appliance("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel", 85000));
            _componentList.Add(new Appliance("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation", 85000));
            _componentList.Add(new Appliance("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder", 85000));
            _componentList.Add(new Appliance("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder", 85000));
            _componentList.Add(new Appliance("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger", 85000));
            _componentList.Add(new Appliance("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel", 85000));
            _componentList.Add(new Appliance("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation", 85000));
            _componentList.Add(new Appliance("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder", 85000));
            _componentList.Add(new Appliance("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder", 85000));
            _componentList.Add(new Appliance("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger", 85000));
            _componentList.Add(new Appliance("Logano plus SB105", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus SB105 RC35", "Oliekedel fra Bosch", "Oliekedel", 85000));
            _componentList.Add(new Appliance("Logano plus GB312", "Gulvmonteret Gaskedel fra Bosch", "Gaskedel", 85000));
            _componentList.Add(new Appliance("Compress 6000 LW", "Brine/vand Varmepumpe fra Bosch", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("AWM", "Luft/vand indendørs varmepumpe", "Varmepumpe", 85000));
            _componentList.Add(new Appliance("CW400 Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("CFB Bosch", "Temperaturregulator fra Bosch", "Temperaturstyring", 85000));
            _componentList.Add(new Appliance("AGS10-2 Bosch", "Uden varmeveksler", "Solvarmestation", 85000));
            _componentList.Add(new Appliance("Logalus SM 300/5", "Bivalent 290L", "Varmtvandsbeholder", 85000));
            _componentList.Add(new Appliance("BST 120-5 SrE", "489 L - 970 L", "Bufferbeholder", 85000));
            _componentList.Add(new Appliance("Logasol SKN4.0 s", "2,25\u00B2", "Solfanger", 85000));

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

        public static Page OfferPage
        {
            get
            {
                return _offerPage == null ? _offerPage = new OfferPage() : _offerPage;
            }
        }

        public static Page ExistingOffersPage
        {
            get
            {
                return _existingOffersPage == null ? _existingOffersPage = new ExistingOffersPage() : _existingOffersPage;
            }
        }
    }

}
