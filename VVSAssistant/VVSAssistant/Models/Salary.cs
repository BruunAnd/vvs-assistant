using System;
using System.Collections.Generic;

namespace VVSAssistant.Models
{
    public class Salary : UnitPrice
    {
        public string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
    }
}
