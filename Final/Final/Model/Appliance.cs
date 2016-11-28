using Final.Model.Data_sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Model
{
    public enum ApplianceTypes
    {
        Boiler,
        Container,
        Solar
    };
    public class Appliance
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ApplianceTypes _type;
        public ApplianceTypes Type
        {
            get { return _type; }
            set { _type = value; }
        }


        private DataSheet _dataSheet;
        public DataSheet DataSheet
        {
            get { return _dataSheet; }
            set { _dataSheet = value; }
        }
    }
}
