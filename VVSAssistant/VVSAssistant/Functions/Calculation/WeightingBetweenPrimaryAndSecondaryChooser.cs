using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.Functions.Calculation
{
    public class WeightingBetweenPrimaryAndSecondaryChooser
    {
        public WeightingBetweenPrimaryAndSecondaryChooser()
        {
            InitiateDataTables();

            PrimHeatPumpSecBoilerGrid.WriteXml("dtDataxml");
        }

        static public DataTable PrimHeatPumpSecBoilerGrid;


        private void InitiateDataTables()
        { 
             DataTable _primHeatSecBoil = new DataTable();
            _primHeatSecBoil.Columns.Add("Result");
            _primHeatSecBoil.Columns.Add("II no Container");
            _primHeatSecBoil.Columns.Add("II with Container");
            
            float[] row1 = { 0.0f, 1.0f, 1.0f};
            float[] row2 = { 0.1f, 0.70f, 0.63f };
            float[] row3 = { 0.2f, 0.45f, 0.30f };
            float[] row4 = { 0.3f, 0.25f, 0.15f };
            float[] row5 = { 0.4f, 0.15f, 0.06f };
            float[] row6 = { 0.5f, 0.05f, 0.02f };
            float[] row7 = { 0.6f, 0.02f, 0.0f };
            float[] row8 = { 0.7f, 0.0f, 0.0f };

            _primHeatSecBoil.Rows.Add(row1);
            _primHeatSecBoil.Rows.Add(row2);
            _primHeatSecBoil.Rows.Add(row3);
            _primHeatSecBoil.Rows.Add(row4);
            _primHeatSecBoil.Rows.Add(row5);
            _primHeatSecBoil.Rows.Add(row6);
            _primHeatSecBoil.Rows.Add(row7);
            _primHeatSecBoil.Rows.Add(row8);

            PrimHeatPumpSecBoilerGrid = _primHeatSecBoil;
        }
       
    }
}