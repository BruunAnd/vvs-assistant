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
    public class ApplianceViewModel : ViewModelBase, IFilterable
    {
        public readonly Appliance Model;

        private DataSheetViewModel _dataSheet;
        private UnitPriceViewModel _unitPrice;

        public ApplianceViewModel(Appliance model)
        {
            Model = model;
            _dataSheet = new DataSheetViewModel(model.DataSheet);
            _unitPrice = new UnitPriceViewModel(model.UnitPrice);
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

        public ApplianceTypes Type
        {
            get { return Model.Type; }
            set
            {
                if (Model.Type == value) return;
                Model.Type = value;
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
                var applianceEntity = dbContext.Appliances.SingleOrDefault(x => x.Id == Model.Id);
                if (applianceEntity == null) return;
                dbContext.Appliances.Remove(applianceEntity);
                dbContext.SaveChanges();
            }
        }

        public void SaveToDatabse()
        {
            using (var dbContext = new AssistantContext())
            {
                dbContext.Appliances.AddOrUpdate(Model);
                dbContext.SaveChanges();
            }
        }

        public UnitPriceViewModel UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; OnPropertyChanged(); }
        }
    }
}
