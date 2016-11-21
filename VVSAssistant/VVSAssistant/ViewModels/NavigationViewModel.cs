using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class NavigationViewModel: ViewModelBase
    {
        //private INavigationService _navigation = new NavigationService();
        public RelayCommand CreateOffer { get; }
        public NavigationViewModel()
        {
            //CreateOffer = new RelayCommand(x =>
            //{_navigation.GoToCreateOffer();});
        }
    }
}
