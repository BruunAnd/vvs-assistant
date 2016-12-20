using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Models;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    internal class CompanyInfoDialogViewModel : NotifyPropertyChanged
    {
        public CompanyInformation CompanyInformation { get; set; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }
        public bool hasRequiredInformation { get; set; }

        public string Address
        {
            get { return CompanyInformation.Address; }
            set { CompanyInformation.Address = value; OnPropertyChanged(); }
        }
        public string Telephone
        {
            get { return CompanyInformation.Telephone; }
            set { CompanyInformation.Telephone = value; OnPropertyChanged(); }
        }
        public string Cvr
        {
            get { return CompanyInformation.Cvr; }
            set { CompanyInformation.Cvr = value; OnPropertyChanged(); }
        }
        public string Email
        {
            get { return CompanyInformation.Email; }
            set { CompanyInformation.Email = value; OnPropertyChanged(); }
        }
        public string Website
        {
            get { return CompanyInformation.Website; }
            set { CompanyInformation.Website = value; OnPropertyChanged(); }
        }
        public string CompanyName
        {
            get { return CompanyInformation.CompanyName; }
            set { CompanyInformation.CompanyName = value; OnPropertyChanged(); }
        }

        public CompanyInfoDialogViewModel(Action<CompanyInfoDialogViewModel> closeHandler, Action<CompanyInfoDialogViewModel> completionHandler)
        {
            LoadDataFromDatabase();
            PropertyChanged += OnInputReceived;
            OnPropertyChanged(); //Make save button clickable if information is already there

            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);

                // Save changes to database
                using (var ctx = new AssistantContext())
                {
                    ctx.Entry(CompanyInformation).State = EntityState.Modified;
                    ctx.CompanyInformation.AddOrUpdate(CompanyInformation);
                    ctx.SaveChanges();
                }
            }, x => hasRequiredInformation);

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }

        private void OnInputReceived(object source, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Telephone) ||
                string.IsNullOrEmpty(Cvr)     || string.IsNullOrEmpty(Email)     ||
                string.IsNullOrEmpty(Website) || string.IsNullOrEmpty(CompanyName))
            {
                hasRequiredInformation = false;
                return;
            }
            hasRequiredInformation = true;

            SaveCommand?.NotifyCanExecuteChanged();
        }

        public void LoadDataFromDatabase()
        {
            using (var ctx = new AssistantContext())
                CompanyInformation = ctx.CompanyInformation.FirstOrDefault();

            // Create new it does not exist
            if (CompanyInformation == null)
                CompanyInformation = new CompanyInformation();
        }
    }
}
