using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VVSAssistant.ViewModels.MVVM;
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

namespace VVSAssistant.ViewModels
{
    class CreateOfferViewModel : ViewModelBase
    {
        public RelayCommand PrintNewOffer { get; }
        public RelayCommand SolutionDoubleClicked { get; }
        public RelayCommand CreateNewOffer { get; }
        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions { get; }
        private ObservableCollection<ClientViewModel> _clients;
        private OfferViewModel _offer;
        public OfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; OnPropertyChanged(); }
        }
        private IDialogCoordinator _dialogCoordinator;
        private bool isComponentTabVisible;
        public bool IsComponentTabVisible
        {
            get { return isComponentTabVisible; }
            set { isComponentTabVisible = value; OnPropertyChanged(); }
        }
        private bool arePackagedSolutionsVisible;
        public bool ArePackagedSolutionsVisible
        {
            get { return arePackagedSolutionsVisible; }
            set { arePackagedSolutionsVisible = value; OnPropertyChanged(); }
        }
        public PackagedSolutionViewModel SelectedPackagedSolution
        {
            get { return Offer.PackagedSolution; }
            set { Offer.PackagedSolution = value; OnPropertyChanged(); }
        }

        public CreateOfferViewModel(IDialogCoordinator coordinator)
        {
            #region Properties and fields

            _offer = new OfferViewModel(new Offer());
            _dialogCoordinator = coordinator;
            PackagedSolutions = new ObservableCollection<PackagedSolutionViewModel>();
            #endregion

            #region Commands

            /* Tied to "print offer" button in bottom left corner. 
             * Disabled if VerifyOfferHasRequiredInformation returns false. */
            PrintNewOffer = new RelayCommand
                        (x => PrintOfferDialog(), 
                         x => VerifyOfferHasRequiredInformation()); 

            /* Tied to the action of double clicking a packaged solution's info 
             * in the list of packaged solutions. When this happens, property 
             * "SelectedPackagedSolution" is set to the clicked Packaged Solution. */
            SolutionDoubleClicked = new RelayCommand
                        (x => OnSolutionDoubleClicked()); 

            /* When the "nyt tilbud" button in bottom left corner is pressed. 
             * Nullifies all offer properties and changes view to list of packaged solutions. */
            CreateNewOffer = new RelayCommand
                        ( x => SetInitialSettings()); 

            #endregion

            #region Initial view settings

            SetInitialSettings();

            #endregion

            #region Events
            Offer.Materials.CollectionChanged += NotifyOfferContentsChanged;
            Offer.Salaries.CollectionChanged += NotifyOfferContentsChanged;
            #endregion

            #region Fetch from database

            using (var dbContext = new AssistantContext())
            {
                dbContext.PackagedSolutions.ToList().ForEach(p => PackagedSolutions.Add(new PackagedSolutionViewModel(p)));
            }

            #endregion
        }

        #region Methods

        /* Initializes the view */
        public void SetInitialSettings()
        {
            ArePackagedSolutionsVisible = true;
            IsComponentTabVisible = false;

            Offer = new OfferViewModel(new Offer());
            SelectedPackagedSolution = null;
            PrintNewOffer.NotifyCanExecuteChanged();
        }

        public bool VerifyOfferHasRequiredInformation()
        {
            if (Offer.PackagedSolution != null &&
                Offer.Salaries.Count   != 0    &&
                Offer.Materials.Count  != 0 )
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
            var dialogViewModel = new GenerateOfferDialogViewModel(Offer, _clients, _dialogCoordinator, instance =>
            {
                // Makes it possible to close the dialog.
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });
            customDialog.Content = new GenerateOfferDialogView { DataContext = dialogViewModel };
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        /* When items are added to Offer.Materials and Offer.Salaries */
        private void NotifyOfferContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PrintNewOffer.NotifyCanExecuteChanged();
        }


        #endregion

    }
}
