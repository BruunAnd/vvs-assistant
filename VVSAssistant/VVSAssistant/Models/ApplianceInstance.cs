namespace VVSAssistant.Models
{
    /* This class is necessary due to limitations on the database.
     * Without this class we would not be able to add multiple
     * instances of the same appliance to a specific packaged
     * solution, because there would be two rows containing the 
     * same foreign keys.*/
    public class ApplianceInstance
    {
        public ApplianceInstance() {}

        public ApplianceInstance(Appliance appliance)
        {
            Appliance = appliance;
            //DataSheet = appliance?.DataSheet;
        }

        public int Id { get; set; }
        public Appliance Appliance { get; set; }
        // public DataSheet DataSheet { get; set;  }
    }
}
