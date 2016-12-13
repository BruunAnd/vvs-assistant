using VVSAssistant.Models.Interfaces;

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
        }

        public ApplianceInstance MakeCopy()
        {
            return new ApplianceInstance(Appliance)
            {
                IsPrimary = IsPrimary,
                IsSolarContainer = IsSolarContainer,
                IsUsedForRoomHeating = IsUsedForRoomHeating,
                IsUsedForWaterHeating = IsUsedForWaterHeating
            };
        }

        public int Id { get; set; }
        public Appliance Appliance { get; set; }
        public bool IsUsedForWaterHeating { get; set; }
        public bool IsUsedForRoomHeating { get; set; }
        public bool IsSolarContainer { get; set; }
        public bool IsPrimary { get; set; }
    }
}
