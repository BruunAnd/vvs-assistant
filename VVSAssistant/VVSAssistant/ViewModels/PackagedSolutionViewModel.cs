using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.Interfaces;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    public class PackagedSolutionViewModel : ViewModelBase, IFilterable
    {
        public readonly PackagedSolution Model;

        public ObservableCollection<ApplianceViewModel> Appliances { get; set; }

        public PackagedSolutionViewModel() : this(new PackagedSolution()) { }

        public PackagedSolutionViewModel(PackagedSolution model)
        {
            Model = model;

            // Transform list of Appliance to list of ApplianceViewModel
            Appliances = new ObservableCollection<ApplianceViewModel>();
            foreach (var appliance in Model.Appliances)
            {
                Appliances.Add(new ApplianceViewModel(appliance));
                OnPropertyChanged(tempNmbrAppliances);
            }
        }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (Model.Name == value) return;
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        public string tempNmbrAppliances
        {
            get { return Appliances.Count.ToString(); }
        }

        public string CreationDate => Model.CreationDate.ToString(@"dd\/MM\/yyyy HH:mm");

        public string Description => string.Join(", ", Appliances);

        public bool DoesFilterMatch(string query)
        {
            if (Name.ContainsIgnoreCase(query) || CreationDate.ContainsIgnoreCase(query))
                return true;
            // Checks if any of the contained appliances match the filter
            return Appliances.Cast<IFilterable>().Any(x => x.DoesFilterMatch(query));
        }

        public void SaveToDatabase()
        {
            using (var dbContext = new AssistantContext())
            {
                Model.Appliances = new ApplianceList(Appliances.Select(x => x.Model).ToList()); // Update model to reflect changes
                Model.CreationDate = DateTime.Now;

                dbContext.PackagedSolutions.AddOrUpdate(Model);
                dbContext.SaveChanges();
            }
        }
    }
}
