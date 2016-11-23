using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    public class DataSheetViewModel : ViewModelBase
    {
        private readonly DataSheet _dataSheet;

        public DataSheetViewModel(DataSheet dataSheet)
        {
            _dataSheet = dataSheet;
        }

        public int Id
        {
            get { return _dataSheet.Id; }
            set
            {
                _dataSheet.Id = value;
                OnPropertyChanged();
            }
        }

        public double Price
        {
            get { return _dataSheet.Price; }
            set
            {
                _dataSheet.Price = value;
                OnPropertyChanged();
            }
        }
    }
}
