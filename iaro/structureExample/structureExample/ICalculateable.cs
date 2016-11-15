using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace structureExample
{
    interface ICalculateable
    {
        // All data requried for calculation
        int DataAtt1 { get; set; }
        int DataAtt2 { get; set; }
        IDataSheet DataSheet { get; set; }
    }
}
