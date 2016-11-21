using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VVSAssistant.Models;
using VVSAssistant.Views;

namespace VVSAssistant.ViewModels.MVVM
{
    class NavigationService:INavigationService
    {
        public event EventHandler<DataEventArgs> DataSent;

        private Page _currentPage;



        public NavigationService()
        {
        }

        public void Goback()
        {
            throw new NotImplementedException();
        }

        public void GoToCreatePackageSolution()
        {
            _currentPage = new CreatePackagedSolutionView();
        }

        public void GoToEditPackageSolution(PackagedSolution p)
        {
            //currentPage = new CreatePackagedSolutionView(p);
        }

        public void GoToCreateOffer()
        {
            throw new NotImplementedException();
        }

        public void GoToExistingOffers()
        {
            throw new NotImplementedException();
        }

        public void GoToExistingPackageSolutions()
        {
            _currentPage = new ExistingPackagedSolutionsView();
        }

        public void GoToNavigation()
        {
            _currentPage = new NavigationView();
            
        }
    }
}
