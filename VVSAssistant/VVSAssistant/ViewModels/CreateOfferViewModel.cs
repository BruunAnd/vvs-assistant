using System;
using System.Collections.ObjectModel;
using VVSAssistant.Models;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.Controls.Dialogs.ViewModels;
using VVSAssistant.Controls.Dialogs.Views;
using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Functions;

namespace VVSAssistant.ViewModels
{
    internal class CreateOfferViewModel : ViewModelBase
    {
        #region RelayCommands
        
        public RelayCommand NavigateBackCmd { get; }
        public RelayCommand PrintOfferCmd { get; }
        public RelayCommand PackagedSolutionSelectedCmd { get; }
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

        public ObservableCollection<UnitPrice> MaterialsInOffer { get; set; }

        public ObservableCollection<UnitPrice> SalariesInOffer { get; set; }

        public ObservableCollection<UnitPrice> AppliancesInOffer { get; set; }

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
        public PackagedSolution SelectedPackagedSolution { get; set; }

        #endregion

        public CreateOfferViewModel(IDialogCoordinator coordinator)
        {
            SetInitialSettings();
            _dialogCoordinator = coordinator;

            PackagedSolutions = new ObservableCollection<PackagedSolution>();
            MaterialsInOffer = new ObservableCollection<UnitPrice>();
            SalariesInOffer = new ObservableCollection<UnitPrice>();
            AppliancesInOffer = new ObservableCollection<UnitPrice>();

            // Assign notifier to the collections we want to monitor.
            MaterialsInOffer.CollectionChanged += NotifyOfferContentsChanged;
            SalariesInOffer.CollectionChanged += NotifyOfferContentsChanged;
            AppliancesInOffer.CollectionChanged += NotifyOfferContentsChanged;

            NavigateBackCmd = new RelayCommand(async x =>
            {
                if (!IsDataSaved)
                {
                    var result = await NavigationService.ConfirmDiscardChanges(_dialogCoordinator);
                    if (result == false) return;
                }
                NavigationService.GoBack();
            });
            
            /* Tied to the action of double clicking a packaged solution's info 
             * in the list of packaged solutions. When this happens, property 
             * "SelectedPackagedSolution" is set to the clicked Packaged Solution. */
            PackagedSolutionSelectedCmd = new RelayCommand(x => OnSolutionSelected(), x => SelectedPackagedSolution != null); 

            /* Runs the dialog if no offerinformation is present and saves to database directly if it is */
            SaveOfferCmd = new RelayCommand(x =>
            {
                if (Offer.OfferInformation == null)
                {
                    SaveOfferDialog();
                }
                else
                {
                    SaveOfferToDatabase(Offer);
                }
            }, x => VerifyOfferHasRequiredInformation());

            PrintOfferCmd = new RelayCommand(x =>
            {
                SaveOfferToDatabase(Offer);
                ExportOffer();
            }, x => VerifyOfferHasRequiredInformation() && Offer.OfferInformation != null);

            /* When the "nyt tilbud" button in bottom left corner is pressed. 
             * Nullifies all offer properties and changes view to list of packaged solutions. */
            CreateNewOfferCmd = new RelayCommand(x =>
            {
                SetInitialSettings();
                ClearCollections();
                // PackagedSolutions.Clear();
                NotifyCanExecuteChanged();
            }, x => ArePackagedSolutionsVisible == false);
        }

        #region Methods

        /* Initializes the view */
        private void SetInitialSettings()
        {
            ArePackagedSolutionsVisible = true;
            IsComponentTabVisible = false;
            Offer = new Offer();
            UpdateSidebarValues();
        }

        private void ClearCollections()
        {
            // Setting to a new collection instance as clearing the collection causes 
            // the packaged solutions appliance list to be cleared in runtime (reference values). *Important
            AppliancesInOffer.Clear();
            MaterialsInOffer.Clear();
            SalariesInOffer.Clear();
        }

        private bool VerifyOfferHasRequiredInformation()
        {
            return Offer.PackagedSolution != null &&
                   SalariesInOffer.Count   != 0    &&
                   MaterialsInOffer.Count  != 0;
        }

        /* Enables the Component, Salary, and Materials view, and prepares
         * the offer for receiving information about any of these */
        private void OnSolutionSelected()
        {
            IsDataSaved = false;
            IsComponentTabVisible = true;
            ArePackagedSolutionsVisible = false;

            // Set offer's packaged solution to selected
            Offer.PackagedSolution = SelectedPackagedSolution;

            // Add appliances to appliances in this offer
            foreach (var appliance in SelectedPackagedSolution.Appliances)
                AppliancesInOffer.Add(new UnitPrice(appliance));

            UpdateSidebarValues();

            NotifyCanExecuteChanged();
        }

        public void SelectPackagedSolutionById(int id)
        {
            SelectedPackagedSolution = PackagedSolutions.FirstOrDefault(p => p.Id == id);
            if (SelectedPackagedSolution == null)
                return;
            OnSolutionSelected();
        }

        /* Opens offer creation dialog */
        private void SaveOfferDialog()
        {
            RunGenerateOfferDialog();
        }

        /* Called by PrintOfferDialog */
        private async void RunGenerateOfferDialog()
        {
            var customDialog = new CustomDialog();
            var dialogViewModel = new GenerateOfferDialogViewModel(Offer,
                closeHandler =>
                {
                    // Closes the dialog
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    Offer.OfferInformation = null;
                    NotifyCanExecuteChanged();
                }, async completionHandler =>
                {
                Offer.TotalCostPrice = TotalCostPrice;
                Offer.TotalContributionMargin = TotalContributionMargin;
                SaveOfferToDatabase(Offer);

                NotifyCanExecuteChanged();

                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                DisplayTimedMessage("Succes", $"Tilbuddet \"{ Offer.OfferInformation.Title}\" blev gemt.", 2);
                },
                printHandler =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    Offer.TotalCostPrice = TotalCostPrice;
                    Offer.TotalContributionMargin = TotalContributionMargin;
                    SaveOfferToDatabase(Offer);
                    ExportOffer();
                    NotifyCanExecuteChanged();
                });

            customDialog.Content = new GenerateOfferDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        public void ExportOffer()
        {
            DataUtil.Offer.Export(Offer);
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
            NotifyCanExecuteChanged();
            IsDataSaved = false;
        }

        private void NotifyCanExecuteChanged()
        {
            PrintOfferCmd?.NotifyCanExecuteChanged();
            SaveOfferCmd?.NotifyCanExecuteChanged();
            CreateNewOfferCmd?.NotifyCanExecuteChanged();
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
            using (var ctx = new AssistantContext())
            {
                ctx.PackagedSolutions
                    .Include(p => p.ApplianceInstances
                    .Select(a => a.Appliance.DataSheet))
                    .ToList()
                    .ForEach(p =>
                    {
                        p.LoadFromInstances();
                        PackagedSolutions.Add(p);
                    });
            }
        }

        public void LoadExistingOffer(int existingOfferId)
        {
            using (var ctx = new AssistantContext())
            {
                var existingOffer = ctx.Offers.Where(o => o.Id == existingOfferId)
                    .Include(o => o.Appliances)
                    .Include(o => o.PackagedSolution)
                    .Include(o => o.Materials)
                    .Include(o => o.Salaries)
                    .FirstOrDefault();

                if (existingOffer == null) return;

                Offer.PackagedSolution = existingOffer.PackagedSolution;

                // Load appliances
                foreach (var appliance in existingOffer.Appliances)
                    AppliancesInOffer.Add(appliance);

                // Load materials
                foreach (var material in existingOffer.Materials)
                    MaterialsInOffer.Add(material);

                // Load salaries
                foreach (var salary in existingOffer.Salaries)
                    SalariesInOffer.Add(salary);

                ArePackagedSolutionsVisible = false;
                IsComponentTabVisible = true;
            }
        }

        private void SaveOfferToDatabase(Offer offer)
        {
            using (var ctx = new AssistantContext())
            {
                ctx.PackagedSolutions.Attach(offer.PackagedSolution);
                
                offer.Appliances = AppliancesInOffer.ToList();
                offer.Salaries = SalariesInOffer.ToList();
                offer.Materials = MaterialsInOffer.ToList();

                // Set existing unitprices to modified state
                foreach (var unit in offer.Appliances.Concat(offer.Salaries).Concat(offer.Materials).Where(u => u.Id != 0))
                    ctx.Entry(unit).State = EntityState.Modified;

                if (offer.Client.CreationDate == default(DateTime))
                    offer.Client.CreationDate = DateTime.Now;

                if (offer.CreationDate == default(DateTime))
                    offer.CreationDate = DateTime.Now;

                ctx.Entry(Offer).State = Offer.Id == 0 ? EntityState.Added : EntityState.Modified;

                ctx.SaveChanges();
            }
            IsDataSaved = true;
        }

        private async void DisplayTimedMessage(string title, string message, double time)
        {
            var customDialog = new CustomDialog();
            var messageViewModel = new TimedMessageViewModel(title, message, time, instanceCancel => _dialogCoordinator.HideMetroDialogAsync(this, customDialog));
            customDialog.Content = new TimesMessageView { DataContext = messageViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }
        #endregion
    }
}
