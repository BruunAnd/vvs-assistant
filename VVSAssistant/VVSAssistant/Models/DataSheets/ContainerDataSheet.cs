using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Functions.Calculation.Interfaces;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for beholder")]
    class ContainerDataSheet : DataSheet
    {
        [NotMapped]
        public static Dictionary<string, float> ClassificationClass = new Dictionary<string, float>()
        {
            {"A*", 0.95f }, {"A", 0.91f }, {"B", 0.86f}, {"C", 0.83f}, {"D", 0.81f}, {"E", 0.81f}, {"F", 0.81f}, {"G", 0.81f}
        };

        [DisplayName(@"Volumen")]
        public float Volume { get; set; }
        [DisplayName(@"Klassificering")]
        public string Classification { get; set; }
        [DisplayName(@"Stilstandstab")]
        public float StandingLoss { get; set; }

        [Browsable(false)]
        public bool isWaterContainer { get; set; }
        [Browsable(false)]
        public bool isBufferContainer { get; set; }
        [Browsable(false)]
        public bool isBivalent { get; set; }
        [Browsable(false)]
        public bool isMonovalent { get; set; }



        public override string ToString()
        {
            string waterContainer = "";
            string bufferContainer = "";
            string Bivalent = "";
            string Monovalent = "";

            if (isWaterContainer == true)
                waterContainer = ", varmtvandsbeholder";
            if (isBufferContainer == true)
                bufferContainer = ", bufferbeholder";
            if (isBivalent == true)
                Bivalent = ", Bivalent";
            if (isMonovalent == true)
                Monovalent = ", Monovalent";

            return $"Volume: {Volume}L, energiklasse: {Classification}, stilstandstab: {StandingLoss}W{waterContainer}{bufferContainer}{Bivalent}{Monovalent}";
        }
    }
}
