using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    class ContainerDataSheet : DataSheet
    {
        [NotMapped]
        public static Dictionary<string, float> ClassificationClass = new Dictionary<string, float>()
        {
            {"A*", 0.95f }, {"A", 0.91f }, {"B", 0.86f}, {"C", 0.83f}, {"D", 0.81f}, {"E", 0.81f}, {"F", 0.81f}, {"G", 0.81f}
        };
        public float Volume { get; set; }
        public string Classification { get; set; }
        public float StandingLoss { get; set; }
    }
}
