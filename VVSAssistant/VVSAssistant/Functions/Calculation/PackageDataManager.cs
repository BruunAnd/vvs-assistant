using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;

namespace VVSAssistant.Functions.Calculation
{
    public class PackageDataManager
    {
        private PackagedSolution _package;
        public PackageDataManager(PackagedSolution package)
        {
            _package = package;
        }
        public HeatingUnitDataSheet PrimaryUnit
        {
            get
            {
                return _package?.PrimaryHeatingUnit.DataSheet as HeatingUnitDataSheet;
            }
        }
    }
}
