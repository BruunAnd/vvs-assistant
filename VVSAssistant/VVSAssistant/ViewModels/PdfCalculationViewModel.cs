using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels
{
    class PdfCalculationViewModel
    {
        public string PageOne { get; set; }
        public string PageTwo { get; set; }
        public string Pagethree { get; set; }
        public string PageFour { get; set; }
        public string PageFive { get; set; }
        

        //Page 1

        public string PackagedAnnualEfficiencyAverageClima { get; set; } // Page 4 og 1

        public string ResultTwo { get; set; }
        //page 2
        public string SupHeatingUnitAnnualEfficiency { get; set; }
        public string SupHeatingUnitTotal { get; set; }

        public string SolarContributionAndSupHeatingUnitTotal { get; set; }

        public string PackagedAnnualEfficiencyRoomHeating { get; set; } // page 3 og 2


        //Page 5

        public string UseProfile { get; set; }
        public string AnnualWaterheatingEfficiency { get; set; }

        //Fælles
        public string PrimAnnualEfficiency { get; set; }

        public string TemperatureControleclass { get; set; }

        public string SupBoilerAnnualEfficiency { get; set; }
        public string SupBoilerTotal { get; set; }

        public string SolarM2 { get; set; }
        public string SolarM3 { get; set; }
        public string SolarEfficiency { get; set; }
        public string SolarClass { get; set; }
        public string SolarTotal { get; set; }

        public int PackagedAnnualEfficiencyEEILabel { get; set; }
        public string ResultOne { get; set; }




    }
}
