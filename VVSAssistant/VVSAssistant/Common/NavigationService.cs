using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Common
{
    public static class NavigationService
    {
        public static void NavigateTo(ViewModelBase page)
        {
            // If a dataconnection for the currentviewmodel is active, close it.
            CurrentPage?.CloseDataConnection();
            
            // Navigate to page
            CurrentPage = page;

            if (page == null)
            {
                NavigationStack.Clear();
                return;
            }
            
            // Open dataconnection and load data from database
            CurrentPage?.OpenDataConnection();
            CurrentPage?.LoadDataFromDatabase();

            // Add page to navigation stack
            NavigationStack.Add(page);
        }

        public static bool GoBack()
        {
            // Navigation failed
            if (!NavigationStack.Any()) return false;

            NavigationStack.Remove(NavigationStack.GetEnumerator().Current);
            NavigateTo(NavigationStack.GetEnumerator().Current);

            // Navigation succeeded
            return true;
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void OnStaticPropertyChanged(string propName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
        }


        private static ViewModelBase _currentPage;
        public static ViewModelBase CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnStaticPropertyChanged("CurrentPage");
            }
        }

        private static readonly ICollection<ViewModelBase> NavigationStack = new List<ViewModelBase>();
    }
}
