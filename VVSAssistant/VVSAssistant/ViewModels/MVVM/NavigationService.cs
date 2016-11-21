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
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value) return;
                _currentPage = value;
                _frame.Navigate(_currentPage);
            }
        }

        private Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void Goback()
        {
            throw new NotImplementedException();
        }

        public void GoToCreatePackageSolution()
        {
            CurrentPage = new CreatePackagedSolutionView();
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
            CurrentPage = new ExistingPackagedSolutionsView();
        }

        public void GoToNavigation()
        {
            CurrentPage = new NavigationView();
        }
    }
}
