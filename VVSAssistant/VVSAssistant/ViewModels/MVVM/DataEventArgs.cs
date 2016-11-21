using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels.MVVM
{
    public class DataEventArgs:EventArgs
    {
        public object Data { get; private set; }

        public DataEventArgs(object data)
        {
            this.Data = data;
        }
    }
}
