using System;
using System.Collections.ObjectModel;
using VVSAssistant.Models;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    internal class CreateOfferViewModel : ViewModelBase
    {
        #region RelayCommands

        public RelayCommand PrintOfferCmd { get; }
        public RelayCommand PackagedSolutionDoubleClickedCmd { get; }
        public RelayCommand CreateNewOfferCmd { get; }
        public RelayCommand SaveOfferCmd { get; }

        #endregion

        #region Calculated properties

        public double TotalSalesPrice => SalariesInOffer.Sum(salary => salary.SalesPrice)
                                         + MaterialsInOffer.Sum(material => material.SalesPrice)
                                         + AppliancesInOffer.Sum(appliance => appliance.SalesPrice);

        public double TotalCostPrice => SalariesInOffer.Sum(salary => salary.CostPrice)
                                         + MaterialsInOffer.Sum(material => material.CostPrice)
                                         + AppliancesInOffer.Sum(appliance => appliance.CostPrice);

        public double TotalContributionMargin => SalariesInOffer.Sum(salary => salary.ContributionMargin)
                                         + MaterialsInOffer.Sum(material => material.ContributionMargin)
                                         + AppliancesInOffer.Sum(appliance => appliance.ContributionMargin);

        public double AppliancesSalesPrice => AppliancesInOffer.Sum(appliance => appliance.SalesPrice);
        public double AppliancesCostPrice => AppliancesInOffer.Sum(appliance => appliance.CostPrice);
        public double AppliancesContributionMargin => AppliancesInOffer.Sum(appliance => appliance.ContributionMargin);

        public double SalariesSalesPrice => SalariesInOffer.Sum(x => x.SalesPrice);
        public double SalariesCostPrice => SalariesInOffer.Sum(x => x.CostPrice);
        public double SalariesContributionMargin => SalariesInOffer.Sum(x => x.ContributionMargin);


        public double MaterialsSalesPrice => MaterialsInOffer.Sum(x => x.SalesPrice);
        public double MaterialsCostPrice => MaterialsInOffer.Sum(x => x.CostPrice);
        public double MaterialsContributionMargin => MaterialsInOffer.Sum(x => x.ContributionMargin);

        #endregion

        #region Collections


        public ObservableCollection<PackagedSolution> PackagedSolutions { get; set; }

        public ObservableCollection<Material> MaterialsInOffer { get; set; }

        public ObservableCollection<Salary> SalariesInOffer { get; set; }

        public ObservableCollection<Appliance> AppliancesInOffer { get; set; }

        #endregion

        #region Properties


        public Offer Offer { get; set; }

        private readonly IDialogCoordinator _dialogCoordinator;

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

        /// <summary>
        /// When a PackagedSolution is selected in the PackagedSolution list,
        /// it will automaticly bind to this property. In the setter, we first
        /// make sure that the property is not null, if it is, we return and do nothing.
        /// The current offers packaged solution is set to the selected packaged solution,
        /// and each appliance is added to the list of appliances in the offer. Last but not least,
        /// we raise a notifier that the property has been changed.
        /// </summary>
        public PackagedSolution SelectedPackagedSolution
        {
            get { return Offer.PackagedSolution; }
            set
            {
                if (value == null) return;
                Offer.PackagedSolution = value;
                foreach (var appliance in value.Appliances)
                    AppliancesInOffer.Add(appliance);
                OnPropertyChanged();
                UpdateSidebarValues();
            }
        }

        #endregion


        public CreateOfferViewModel(IDialogCoordinator coordinator)
        {

            _dialogCoordinator = coordinator;
            PackagedSolutions = new ObservableCollection<PackagedSolution>();
            MaterialsInOffer = new ObservableCollection<Material>();
            SalariesInOffer = new ObservableCollection<Salary>();
            AppliancesInOffer = new ObservableCollection<Appliance>();
            
            // Assign notifier to the collections we want to monitor.
            MaterialsInOffer.CollectionChanged += NotifyOfferContentsChanged;
            SalariesInOffer.CollectionChanged += NotifyOfferContentsChanged;
            AppliancesInOffer.CollectionChanged += NotifyOfferContentsChanged;

            /* Tied to "print offer" button in bottom left corner. 
             * Disabled if VerifyOfferHasRequiredInformation returns false. */
            PrintOfferCmd = new RelayCommand
                        (x => 
                        {
                            if (DbContext.Offers.SingleOrDefault(o => o.Id == Offer.Id) == null) //Not saved
                            {
                                SaveOfferDialog();
                                ExportOffer();
                            }
                            else
                                ExportOffer();
                        }, 
                         x => VerifyOfferHasRequiredInformation()); 

            /* Tied to the action of double clicking a packaged solution's info 
             * in the list of packaged solutions. When this happens, property 
             * "SelectedPackagedSolution" is set to the clicked Packaged Solution. */
            PackagedSolutionDoubleClickedCmd = new RelayCommand
                        (x => OnSolutionDoubleClicked()); 

            /* Doing the same as print offer, todo: figure out what we want to accomplish here */
            SaveOfferCmd = new RelayCommand
                        (x => SaveOfferDialog(),
                         x => VerifyOfferHasRequiredInformation());

            /* When the "nyt tilbud" button in bottom left corner is pressed. 
             * Nullifies all offer properties and changes view to list of packaged solutions. */
            CreateNewOfferCmd = new RelayCommand
                (x =>
            {
                SetInitialSettings();
                ClearCollections();
                PackagedSolutions.Clear();
                LoadDataFromDatabase();
            });
            
            SetInitialSettings();
        }

        #region Methods

        /* Initializes the view */
        public void SetInitialSettings()
        {
            ArePackagedSolutionsVisible = true;
            IsComponentTabVisible = false;
            Offer = new Offer();
            UpdateSidebarValues();
        }

        public void ClearCollections()
        {
            // Setting to a new collection instance as clearing the collection causes 
            // the packaged solutions appliance list to be cleared in runtime (reference values). *Important
            AppliancesInOffer.Clear();
            MaterialsInOffer.Clear();
            SalariesInOffer.Clear();
        }

        public bool VerifyOfferHasRequiredInformation()
        {
            return Offer.PackagedSolution != null &&
                   SalariesInOffer.Count   != 0    &&
                   MaterialsInOffer.Count  != 0;
        }

        /* Enables the Component, Salary, and Materials view, and prepares
         * the offer for receiving information about any of these */
        public void OnSolutionDoubleClicked()
        {
            IsComponentTabVisible = true;
            ArePackagedSolutionsVisible = false;
            PrintOfferCmd.NotifyCanExecuteChanged();
        }

        /* Opens offer creation dialog */
        public void SaveOfferDialog()
        {
            RunGenerateOfferDialog();
        }

        /* Called by PrintOfferDialog */
        public async void RunGenerateOfferDialog()
        {
            var customDialog = new CustomDialog();
            var dialogViewModel = new GenerateOfferDialogViewModel(Offer, 
                closeHandler =>
                {
                    // Closes the dialog
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }, 
                completionHandler =>
                {
                    // Closes the dialog and saves the offer to database
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    Offer.TotalCostPrice = TotalCostPrice;
                    Offer.TotalContributionMargin = TotalContributionMargin;
                    SaveOfferToDatabase(Offer);
                });

            customDialog.Content = new GenerateOfferDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        public void ExportOffer()
        {
            var e = new Exporter();
            e.ExportOffer(Offer);
        }
        

        private void NotifyOfferContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            if (e == null) return;

            // Remove eventlistener from deleted properties.
            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= OfferContentsPropertyChanged;

            // Bind a method to run if a property changed in any of the observed classes.
            if (e.NewItems == null) return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += OfferContentsPropertyChanged;
        }

        private void OfferContentsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSidebarValues();
            PrintOfferCmd.NotifyCanExecuteChanged();
            SaveOfferCmd.NotifyCanExecuteChanged();
        }

        private void UpdateSidebarValues()
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
