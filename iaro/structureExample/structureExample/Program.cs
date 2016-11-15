using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace structureExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Appliance();
            app.DataSheet = new HeapPumpStrategyData();
            app.DataSheet.ImportFromDatabase();
        }
    }
}
