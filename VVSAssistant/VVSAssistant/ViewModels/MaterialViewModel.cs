using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class MaterialViewModel : ViewModelBase
    {
        private readonly Material _material;

        public MaterialViewModel(Material material)
        {
            _material = material;
        }

        public int VvsNumber
        {
            get { return _material.VvsNumber; }
            set
            {
                _material.VvsNumber = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _material.Name; }
            set
            {
                _material.Name = value;
                OnPropertyChanged();
            }
        }
    }
}
