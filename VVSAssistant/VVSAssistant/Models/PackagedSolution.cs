using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using VVSAssistant.Functions.Calculation;
using VVSAssistant.Functions.Calculation.Interfaces;
using VVSAssistant.ValueConverters;

namespace VVSAssistant.Models
{
    public class PackagedSolution : ICopyable
    {
        public PackagedSolution()
        {
            ApplianceInstances = new List<ApplianceInstance>();
            EnergyLabel = new List<EEICalculationResult>();
            SolarContainerInstances = new List<ApplianceInstance>();
            CalculationManager = new CalculationManager();
        }

        private ApplianceList _solarContainers;
        [NotMapped]
        public ApplianceList SolarContainers
        {
            get { return _solarContainers ?? (_solarContainers = new ApplianceList((List<ApplianceInstance>)SolarContainerInstances)); }
            set
            {
                _solarContainers = value;
                SolarContainerInstances = _applianceList.BackingList;
            }
        }

        [NotMapped]
        public Appliance PrimaryHeatingUnit
        {
            get { return PrimaryHeatingUnitInstance?.Appliance; }
            set { PrimaryHeatingUnitInstance = new ApplianceInstance(value); }
        }

        private ApplianceList _applianceList;
        [NotMapped]
        public ApplianceList Appliances
        {
            get { return _applianceList ?? (_applianceList = new ApplianceList((List<ApplianceInstance>) ApplianceInstances)); }
            set
            {
                _applianceList = value;
                ApplianceInstances = _applianceList.BackingList;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ApplianceInstance> SolarContainerInstances { get; set; }
        public ICollection<ApplianceInstance> ApplianceInstances { get; set; }
        public ApplianceInstance PrimaryHeatingUnitInstance { get; private set; }
        public string Description => string.Join(", ", Appliances);
        [NotMapped]
        private CalculationManager CalculationManager { get; }
        [NotMapped]
        public List<EEICalculationResult> EnergyLabel { get; set; }

        public object MakeCopy()
        {
            var copy = Activator.CreateInstance(this.GetType()); //New object of same type
            var properties = this.GetType().GetProperties(); //All properties from this object
            foreach (PropertyInfo pi in copy.GetType().GetProperties()) //For all the properties in the new object
            {
                var matchingProperty = properties.First(p => p.Name == pi.Name); //Find the property with the same name in this object
                pi.SetValue(copy, matchingProperty.GetValue(this)); //Set the new object's property with this name to the value of the same property in this object
            }
            return copy;
        }

        /// <summary>
        /// Recalculates the EnergyLabel property of this object.
        /// </summary>
        public void UpdateEEI()
        {
            List<IEEICalculation> calculations = CalculationManager.SelectCalculationStrategy(this);
            foreach (var calculation in calculations ?? Enumerable.Empty<IEEICalculation>())
                EnergyLabel.Add(calculation?.CalculateEEI(this));
        }
    }
}
