using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;

namespace VVSAssistant.Models.DataSheets
{
    class DataSheetProperty : NotifyPropertyChanged
    {
        public DataSheetProperty(object value)
        {
            Value = value;
            _visibility = false;
        }

        private object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        private bool _visibility;
        public bool Visibility
        {
            get { return _visibility; }
            set { _visibility = value; OnPropertyChanged(); }
        }
    }
}
