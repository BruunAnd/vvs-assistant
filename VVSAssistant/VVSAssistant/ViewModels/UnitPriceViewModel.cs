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
    public class UnitPriceViewModel : ViewModelBase
    {
        private readonly UnitPrice _unitPrice;

        public UnitPriceViewModel(UnitPrice unitPrice)
        {
            _unitPrice = unitPrice;
        }

        public int Id
        {
            get { return _unitPrice.Id; }
            set
            {
                _unitPrice.Id = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get { return _unitPrice.Quantity; }
            set
            {
                _unitPrice.Quantity = value;
                OnPropertyChanged();
            }
        }

        public double UnitCostPrice
        {
            get { return _unitPrice.UnitCostPrice; }
            set
            {
                _unitPrice.UnitCostPrice = value;
                OnPropertyChanged();
            }
        }

        public double UnitSalesPrice
        {
            get { return _unitPrice.UnitSalesPrice; }
            set
            {
                _unitPrice.UnitSalesPrice = value;
                OnPropertyChanged();
            }
        }

        public double SalesPrice => _unitPrice.SalesPrice;

        public double CostPrice => _unitPrice.CostPrice;

        public double ContributionMargin => _unitPrice.ContributionMargin;
    }
}
