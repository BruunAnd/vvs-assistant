using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;

namespace VVSAssistant.Functions.Calculation
{
    public class PackageDataManager
    {
        private PackagedSolution _package;
        public PackageDataManager(PackagedSolution package)
        {
            _package = package;
        }
    }
}
