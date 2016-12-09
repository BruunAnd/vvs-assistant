using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Models;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    internal class CompanyInfoDialogViewModel
    {
        public CompanyInformation CompanyInformation { get; set; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        public CompanyInfoDialogViewModel(Action<CompanyInfoDialogViewModel> closeHandler, Action<CompanyInfoDialogViewModel> completionHandler)
        {
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
            });

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
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
