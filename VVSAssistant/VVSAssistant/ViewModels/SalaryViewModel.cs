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
    class SalaryViewModel : ViewModelBase
    {
        private readonly Salary _salary;

        public SalaryViewModel(Salary salary)
        {
            _salary = salary;
        }

        private string Name
        {
            get { return _salary.Name; }
            set
            {
                _salary.Name = value;
                OnPropertyChanged();
            }
        }

        public Salary Salary
        {
            get { return _salary; }
        }
    }
}
