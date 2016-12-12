using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Models.Interfaces;

namespace VVSAssistant.Models
{
    public class PackagedSolution : ICopyable
    {
        public PackagedSolution()
        {
            ApplianceInstances = new List<ApplianceInstance>();
            EnergyLabel = new List<EEICalculationResult>();
            CalculationManager = new CalculationManager();
        }

        public object MakeCopy()
        {
            var copy = Activator.CreateInstance(GetType()); //New object of same type
            var properties = GetType().GetProperties(); //All properties from this object
            foreach (var pi in copy.GetType().GetProperties()) //For all the properties in the new object
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name); //Find the property with the same name in this object
                pi.SetValue(copy, matchingProperty.GetValue(this)); //Set the new object's property with this name to the value of the same property in this object
            }
            return copy;
        }

        /// <summary>
        /// Recalculates the EnergyLabel property of this object.
        /// </summary>
        public void UpdateEei()
        {
            var calculations = CalculationManager.SelectCalculationStrategy(this);
            foreach (var calculation in calculations ?? Enumerable.Empty<IEEICalculation>())
                EnergyLabel.Add(calculation?.CalculateEEI(this));
        }

        #region Unmapped Properties
        [NotMapped]
        private CalculationManager CalculationManager { get; }
        [NotMapped]
        public List<EEICalculationResult> EnergyLabel { get; set; }
        public ApplianceInstance PrimaryHeatingUnitInstance => ApplianceInstances.FirstOrDefault(a => a.IsPrimary);
        public IEnumerable<ApplianceInstance> SolarContainerInstances => ApplianceInstances.Where(a => a.IsSolarContainer);
        public string Description => string.Join(", ", ApplianceInstances.Select(a => a.Appliance));
        #endregion

        #region Mapped Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ApplianceInstance> ApplianceInstances { get; set; }
        #endregion
    }
}
