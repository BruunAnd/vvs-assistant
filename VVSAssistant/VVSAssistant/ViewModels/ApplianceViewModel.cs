using System;
using System.Data.Entity.Migrations;
using System.Linq;
using VVSAssistant.Database;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.Interfaces;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class ApplianceViewModel : ViewModelBase, IFilterable
    {
        private readonly Appliance _appliance;
        private DataSheetViewModel _dataSheet;

        public ApplianceViewModel(Appliance appliance)
        {
            _appliance = appliance;
            _dataSheet = new DataSheetViewModel(appliance.DataSheet);
        }

        public string Name
        {
            get { return _appliance.Name; }
            set
            {
                if (_appliance.Name == value) return;
                _appliance.Name = value;
                OnPropertyChanged();
            }
        }

        public ApplianceTypes Type
        {
            get { return _appliance.Type; }
            set
            {
                if (_appliance.Type == value) return;
                _appliance.Type = value;
                OnPropertyChanged();
            }
        }

        public string Description => "no description xd (should be datasheet stuff)";

        public bool DoesFilterMatch(string query)
        {
            return Name.ContainsIgnoreCase(query)
                   || Type.ToString().ContainsIgnoreCase(query)
                   || Description.ContainsIgnoreCase(query);
        }

        public DataSheetViewModel DataSheet
        {
            get { return _dataSheet; }
            set
            {
                _dataSheet = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void RemoveFromDatabase()
        {
            using (var dbContext = new AssistantContext())
            {
                var applianceEntity = dbContext.Appliances.SingleOrDefault(x => x.Id == _appliance.Id);
                if (applianceEntity == null) return;
                dbContext.Appliances.Remove(applianceEntity);
                dbContext.SaveChanges();
            }
        }

        public void SaveToDatabse()
        {
            using (var dbContext = new AssistantContext())
            {
                dbContext.Appliances.AddOrUpdate(_appliance);
                dbContext.SaveChanges();
            }
        }
    }
}
