using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace structureExample
{
    interface ICalculation
    {
        float Calculate<T>(T data) where T : class;
    }

    class HeatPumpCalculationStrategy : ICalculation
    {
        public float Calculate<T>(T data) where T : class
        {
            var da = data as IHeatpumpCalculateable;
            da.DataAttri1 = 100;
            throw new NotImplementedException();
        }
    }
    interface IDataSheet
    {
        void ImportFromDatabase();
    }

    interface IHeatpumpCalculateable : IDataSheet
    {
        int DataAttri1 { get; set; }
    }

    class HeapPumpStrategyData : IHeatpumpCalculateable
    {
        public int heatpumpDatAtt1;

        public int DataAttri1
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void ImportFromDatabase()
        {
            heatpumpDatAtt1 = -5000;
        }
    }
}
