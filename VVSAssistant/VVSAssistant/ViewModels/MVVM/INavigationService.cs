using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels.MVVM
{
    public interface INavigationService
    {
        event EventHandler <DataEventArgs> DataSent; 
        void Goback();
        void GoToCreatePackageSolution();
        void GoToEditPackageSolution(Models.PackagedSolution p);

        void GoToCreateOffer();
        void GoToExistingOffers();
        void GoToExistingPackageSolutions();
        void GoToNavigation();
    }
}
