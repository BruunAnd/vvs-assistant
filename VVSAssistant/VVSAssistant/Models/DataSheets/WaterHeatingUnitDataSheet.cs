using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Models.DataSheets
{
    class WaterHeatingUnitDataSheet : DataSheet
    {
        public float WaterHeatingEffiency { get; set; }
        public UseProfileType UseProfile { get; set; }
    }
    public enum UseProfileType
    {
        XXXS = 1, XXS, XS, S, M, L, XL, XXL
    }
}
