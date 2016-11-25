using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Beholder, datablad")]
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
        [DisplayName(@"Stående tab (lel)")]
        public float StandingLoss { get; set; }
    }
}
