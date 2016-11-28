using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VVSAssistant.Database;

namespace VVSAssistant.Models
{
    public class Material : UnitPrice
    {
        [NotMapped]
        public string VvsNumber
        {
            set
            {
                using (var dbContext = new AssistantContext())
                {
                    MaterialReference = dbContext.MaterialReferences.FirstOrDefault(x => x.VvsNumber != null && x.VvsNumber.Equals(value)) ?? new MaterialReference() {Name="new"};
                }
            }
        }

        [NotMapped]
        public string Name => Reference?.Name;

        [NotMapped]
        private MaterialReference MaterialReference
        {
            get { return Reference; }
            set
            {
                Reference = value;
                if (Reference == null) return;
                UnitCostPrice = Reference.CostPrice;
                // todo whenever this is updated, we should update all the columns in CreateOfferView
                OnPropertyChanged("Name");
            }
        }

        public virtual MaterialReference Reference { get; set; }
    }
}
