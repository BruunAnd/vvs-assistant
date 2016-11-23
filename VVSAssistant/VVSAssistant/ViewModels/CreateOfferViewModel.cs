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

namespace VVSAssistant.ViewModels
{
    class CreateOfferViewModel : ViewModelBase
    {
        public RelayCommand CreateNewOffer { get; }
        public RelayCommand SolutionDoubleClicked { get; }
        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions { get; }
        private ObservableCollection<ClientViewModel> _clients;
        private OfferViewModel _offer;
        private IDialogCoordinator _dialogCoordinator;
        private bool isComponentTabVisible;
        private bool arePackagedSolutionsVisible;

        public CreateOfferViewModel(IDialogCoordinator coordinator)
        {
            _offer = new OfferViewModel(new Offer());
            _dialogCoordinator = coordinator;

            CreateNewOffer = new RelayCommand(x => CreateOffer()/*, x => VerifyNeededInformation()*/);
            SolutionDoubleClicked = new RelayCommand(x =>
            {
                IsComponentTabVisible = true;
                ArePackagedSolutionsVisible = false;
            });

            PackagedSolutions = new ObservableCollection<PackagedSolutionViewModel>();
            // Load list of packaged solutions from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of PackagedSolution to a list of PackagedSolutionViewModel
                dbContext.PackagedSolutions.ToList().ForEach(p => PackagedSolutions.Add(new PackagedSolutionViewModel(p)));
            }

            IsComponentTabVisible = false;
            arePackagedSolutionsVisible = true;
        }

        public OfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; }
        }

        public bool IsComponentTabVisible
        {
            get { return isComponentTabVisible; }
            set { isComponentTabVisible = value; OnPropertyChanged(); }
        }

        public bool ArePackagedSolutionsVisible
        {
            get { return arePackagedSolutionsVisible; }
            set { arePackagedSolutionsVisible = value;  OnPropertyChanged(); }
        }

        public PackagedSolutionViewModel SelectedPackagedSolution
        {
            get { return Offer.PackagedSolution; }
            set { Offer.PackagedSolution = value;  OnPropertyChanged(); }
        }

        public void CreateOffer()
        {
            RunGenerateOfferDialog();
        }

        /// <summary>
        /// Uses reflection to check whether or not any of the properties in the passed object is null or empty.
        /// </summary>
        /// <returns></returns>
        private bool IsPropertyNullOrEmpty(object objectToCheck)
        {
            //TODO: Fix this method. Doesn't work for some reason, don't know why. 
            /* Fetch all properties */
            foreach (PropertyInfo pi in objectToCheck.GetType().GetProperties())
            {
                string value = (string) pi.GetValue(objectToCheck);
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if both Client and Packaged Solution has a name
        /// </summary>
        /// <returns></returns>
        public bool VerifyNeededInformationDeprecated()
        {
            if (IsPropertyNullOrEmpty(Offer.Client.ClientInformation) ||
                IsPropertyNullOrEmpty(Offer.Client)                   ||
                IsPropertyNullOrEmpty(Offer.PackagedSolution)         ||
                IsPropertyNullOrEmpty(Offer))
                return false;
            else
                return true;
        }

        public bool VerifyNeededInformation()
        {
            if (Offer.Client.ClientInformation.Name == null ||
                Offer.PackagedSolution.Name         == null ||
                Offer.OfferInformation              == null )
            {
                return false;
            }
            else
                return true;
        }

        public async void RunGenerateOfferDialog()
        {
            var customDialog = new CustomDialog();
            var dialogViewModel = new GenerateOfferDialogViewModel(Offer, _clients, _dialogCoordinator, instance =>
            {
                // Makes it possible to close the dialog.
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });
            customDialog.Content = new GenerateOfferDialogView { DataContext = dialogViewModel };
            // await _dialogCoordinator.ShowMessageAsync(this, "bla", "bla");
            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }
    }
}
