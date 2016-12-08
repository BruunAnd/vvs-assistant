using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for beholder")]
    internal class ContainerDataSheet : DataSheet
    {
        [NotMapped]
        public static Dictionary<string, float> ClassificationClass = new Dictionary<string, float>()
        {
            {"0", 0.0f }, {"A*", 0.95f }, {"A", 0.91f }, {"B", 0.86f}, {"C", 0.83f}, {"D", 0.81f}, {"E", 0.81f}, {"F", 0.81f}, {"G", 0.81f}
        };

        [DisplayName(@"Volumen")]
        [Description(@"BeholderVolumen (V) i liter")]
        public float Volume { get; set; }
        [DisplayName(@"Klassificering")]
        [Description(@"Beholderens energiklasse skal være mellem A+ og G")]
        public string Classification { get; set; }
        [DisplayName(@"Stilstandstab")]
        [Description(@"Beholderens stilstandstab (S) i Watt (W)")]
        public float StandingLoss { get; set; }

        [Browsable(false)]
        public bool IsWaterContainer { get; set; }
        [Browsable(false)]
        public bool IsBufferContainer { get; set; }
        [Browsable(false)]
        public bool IsBivalent { get; set; }
        [Browsable(false)]
        public bool IsMonovalent { get; set; }



        public override string ToString()
        {
            var waterContainer = "";
            var bufferContainer = "";
            var bivalent = "";
            var monovalent = "";

            if (IsWaterContainer)
                waterContainer = ", Varmtvandsbeholder";
            if (IsBufferContainer)
                bufferContainer = ", Bufferbeholder";
            if (IsBivalent)
                bivalent = ", Bivalent";
            if (IsMonovalent)
                monovalent = ", Monovalent";

            return $"Volume: {Volume}L, Energiklasse: {Classification}, Stilstandstab: {StandingLoss}W{waterContainer}{bufferContainer}{bivalent}{monovalent}";
        }
    }
}
