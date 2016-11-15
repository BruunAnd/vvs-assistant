using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace structureExample
{
    class CalculationManager
    {
        ICalculation GetCalcFunc(List<ICalculateable> data)
        {
            ICalculation func = new HeatPumpCalculationStrategy();
            HeapPumpStrategyData Heat = new HeapPumpStrategyData();
            Heat.heatpumpDatAtt1 = 2;
            func.Calculate<HeapPumpStrategyData>(Heat);
            return func;
        }
    }
}
