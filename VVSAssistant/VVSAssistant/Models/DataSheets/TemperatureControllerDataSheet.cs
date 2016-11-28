using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VVSAssistant.Models.DataSheets
{
    [DisplayName(@"Datablad for temperaturregulering")]
    public class TemperatureControllerDataSheet : DataSheet
    {
        [NotMapped]
        public static Dictionary<string, float> ClassBonus = new Dictionary<string, float>()
        {
            {"0", 0.0f }, {"1", 1.0f }, {"2", 2.0f }, {"3", 1.5f }, {"4", 2.0f }, {"5", 3.0f }, {"6", 4.0f }, {"7", 3.5f }, {"8", 5.0f }
        };
        [DisplayName(@"Energiklasse")]
        public string Class { get; set; } 
    }
}
