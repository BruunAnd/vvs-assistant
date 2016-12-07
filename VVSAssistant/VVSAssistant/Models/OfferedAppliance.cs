namespace VVSAssistant.Models
{
    public class OfferedAppliance : UnitPrice
    {
        /* Must have an empty constructor for EF6.. */
        public OfferedAppliance() {}

        public OfferedAppliance(Appliance appliance)
        {
            Quantity = 1;
            UnitCostPrice = appliance.DataSheet.Price;
            Appliance = appliance;
        }

        public Appliance Appliance { get; set; }
    }
}
