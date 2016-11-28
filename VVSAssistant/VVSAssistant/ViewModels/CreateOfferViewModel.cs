using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VVSAssistant.Exceptions;
using VVSAssistant.Models;
using System.Reflection;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using VVSAssistant.Database;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;
using System.ComponentModel;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Events;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    class CreateOfferViewModel : ViewModelBase
    {
        #region Properties
        public RelayCommand PrintNewOffer { get; }
        public RelayCommand SolutionDoubleClicked { get; }
        public RelayCommand CreateNewOffer { get; }
        public RelayCommand SaveOffer { get; }
        private ObservableCollection<PackagedSolution> _packagedSolutions;
        public ObservableCollection<PackagedSolution> PackagedSolutions
        {
            get { return _packagedSolutions; }
            set { _packagedSolutions = value; OnPropertyChanged(); }
        }

        public double TotalSalesPrice => SalariesInOffer.Sum(salary => salary.SalesPrice)
                                         + MaterialsInOffer.Sum(material => material.SalesPrice)
                                         + AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.SalesPrice);

        public double TotalCostPrice => SalariesInOffer.Sum(salary => salary.CostPrice)
                                         + MaterialsInOffer.Sum(material => material.CostPrice)
                                         + AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.CostPrice);

        public double TotalContributionMargin => SalariesInOffer.Sum(salary => salary.ContributionMargin)
                                         + MaterialsInOffer.Sum(material => material.ContributionMargin)
                                         + AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.ContributionMargin);

        public double AppliancesSalesPrice => AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.SalesPrice);
        public double AppliancesCostPrice => AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.CostPrice);
        public double AppliancesContributionMargin => AppliancesInPackagedSolution.Sum(appliance => appliance.UnitPrice.ContributionMargin);

        public double SalariesSalesPrice => SalariesInOffer.Sum(x => x.SalesPrice);
        public double SalariesCostPrice => SalariesInOffer.Sum(x => x.CostPrice);
        public double SalariesContributionMargin => SalariesInOffer.Sum(x => x.ContributionMargin);


        public double MaterialsSalesPrice => MaterialsInOffer.Sum(x => x.SalesPrice);
        public double MaterialsCostPrice => MaterialsInOffer.Sum(x => x.CostPrice);
        public double MaterialsContributionMargin => MaterialsInOffer.Sum(x => x.ContributionMargin);


        private ObservableCollection<Client> _clients;
        private Offer _offer;
        public Offer Offer
        {
            get { return _offer; }
            set
            {
                _offer = value;
                OnPropertyChanged();
            }
        }
        private IDialogCoordinator _dialogCoordinator;
        private bool _isComponentTabVisible;
        public bool IsComponentTabVisible
        {
            get { return _isComponentTabVisible; }
            set { _isComponentTabVisible = value; OnPropertyChanged(); }
        }
        private bool _arePackagedSolutionsVisible;
        public bool ArePackagedSolutionsVisible
        {
            get { return _arePackagedSolutionsVisible; }
            set { _arePackagedSolutionsVisible = value; OnPropertyChanged(); }
        }
        public PackagedSolution SelectedPackagedSolution
        {
            get { return Offer.PackagedSolution; }
            set
            {
                Offer.PackagedSolution = value;
                if (value != null)
                {
                    AppliancesInPackagedSolution = new ObservableCollection<Appliance>(value.Appliances);
                }
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Material> _materialsInOffer;
        public ObservableCollection<Material> MaterialsInOffer
        {
            get { return _materialsInOffer; }
            set { _materialsInOffer = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Salary> _salariesInOffer;
        public ObservableCollection<Salary> SalariesInOffer
        {
            get { return _salariesInOffer; }
            set { _salariesInOffer = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Appliance> _appliancesInPackagedSolution;
        public ObservableCollection<Appliance> AppliancesInPackagedSolution
        {
            get { return _appliancesInPackagedSolution; }
            set { _appliancesInPackagedSolution = value; OnPropertyChanged(); }
        }
        #endregion

        public CreateOfferViewModel(IDialogCoordinator coordinator)
        {
            _offer = new Offer();
            _dialogCoordinator = coordinator;
            PackagedSolutions = new ObservableCollection<PackagedSolution>();
            MaterialsInOffer = new ObservableCollection<Material>();
            SalariesInOffer = new ObservableCollection<Salary>();
            AppliancesInPackagedSolution = new ObservableCollection<Appliance>();

            /* Tied to "print offer" button in bottom left corner. 
             * Disabled if VerifyOfferHasRequiredInformation returns false. */
            PrintNewOffer = new RelayCommand
                        (x => PrintOfferDialog(), 
                         x => VerifyOfferHasRequiredInformation()); 

            /* Tied to the action of double clicking a packaged solution's info 
             * in the list of packaged solutions. When this happens, property 
             * "SelectedPackagedSolution" is set to the clicked Packaged Solution. */
            SolutionDoubleClicked = new RelayCommand
                        (x => OnSolutionDoubleClicked(),
                         x => SelectedPackagedSolution != null); 

            /* When the "Gem tilbud" button is pressed, saves the offer if offer has the required information 
             todo add dialog to name the offer */
            SaveOffer = new RelayCommand
                        (x => SaveOfferToDatabase(Offer),
                         x => VerifyOfferHasRequiredInformation());

            /* When the "nyt tilbud" button in bottom left corner is pressed. 
             * Nullifies all offer properties and changes view to list of packaged solutions. */
            CreateNewOffer = new RelayCommand
                        ( x => SetInitialSettings()); 
            
            MaterialsInOffer.CollectionChanged += NotifyOfferContentsChanged;
            SalariesInOffer.CollectionChanged += NotifyOfferContentsChanged;
            AppliancesInPackagedSolution.CollectionChanged += NotifyOfferContentsChanged;
            SetInitialSettings();
        }

        #region Methods

        /* Initializes the view */
        public void SetInitialSettings()
        {
            ArePackagedSolutionsVisible = true;
            IsComponentTabVisible = false;

            Offer = new Offer();
            SelectedPackagedSolution = null;
            PrintNewOffer.NotifyCanExecuteChanged();
        }

        public bool VerifyOfferHasRequiredInformation()
        {
            if (Offer.PackagedSolution != null &&
                SalariesInOffer.Count   != 0    &&
                MaterialsInOffer.Count  != 0 )
                return true;
            else
                return false;
        }

        /* Enables the Component, Salary, and Materials view, and prepares
         * the offer for receiving information about any of these */
        public void OnSolutionDoubleClicked()
        {
            IsComponentTabVisible = true;
            ArePackagedSolutionsVisible = false;
            PrintNewOffer.NotifyCanExecuteChanged();
        }

        /* Opens offer creation dialog */
        public void PrintOfferDialog()
        {
            RunGenerateOfferDialog();
        }

        /* Called by PrintOfferDialog */
        public async void RunGenerateOfferDialog()
        {
            var customDialog = new CustomDialog();
            var dialogViewModel = new GenerateOfferDialogViewModel(Offer, _clients, 
                closeHandler =>
                {
                    // Closes the dialog
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }, 
                completionHandler =>
                {
                    // Closes the dialog and saves the offer to database
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    SaveOfferToDatabase(Offer);
                    Exporter e = new Exporter();
                    e.ExportOffer(Offer);
                });

            customDialog.Content = new GenerateOfferDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        /* When items are added to Offer.Materials and Offer.Salaries */
        private void NotifyOfferContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null) return;

            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= OfferContentsPropertyChanged;

            if (e.NewItems == null) return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += OfferContentsPropertyChanged;
        }

        private void OfferContentsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("TotalSalesPrice");
            OnPropertyChanged("TotalCostPrice");
            OnPropertyChanged("TotalContributionMargin");
            OnPropertyChanged("AppliancesSalesPrice");
            OnPropertyChanged("AppliancesCostPrice");
            OnPropertyChanged("AppliancesContributionMargin");
            OnPropertyChanged("SalariesSalesPrice");
            OnPropertyChanged("SalariesCostPrice");
            OnPropertyChanged("SalariesContributionMargin");
            OnPropertyChanged("MaterialsSalesPrice");
            OnPropertyChanged("MaterialsCostPrice");
            OnPropertyChanged("MaterialsContributionMargin");
            PrintNewOffer.NotifyCanExecuteChanged();
            SaveOffer.NotifyCanExecuteChanged();
        }

        public override void LoadDataFromDatabase()
        {
            DbContext.PackagedSolutions.ToList().ForEach(p => PackagedSolutions.Add(p));
        }

        private void SaveOfferToDatabase(Offer offer)
        {
            offer.Salaries = SalariesInOffer.ToList();
            offer.Materials = MaterialsInOffer.ToList();
            offer.CreationDate = DateTime.Now;
            offer.Client.CreationDate = DateTime.Now;
            DbContext.Offers.Add(offer);
            DbContext.SaveChanges();
        }
        #endregion
    }
}
