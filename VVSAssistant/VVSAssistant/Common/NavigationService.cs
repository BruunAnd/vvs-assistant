using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.ViewModels;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;

namespace VVSAssistant.Common
{
    public static class NavigationService
    {
        public delegate void LoadingStateChangedHandler(bool isLoading);
        public static event LoadingStateChangedHandler LoadingStateChanged;

        public static async Task BeginNavigate(ViewModelBase target)
        {
            // Invoke an event to indicate loading
            LoadingStateChanged?.Invoke(true);

            // On navigation to main page
            if (target == null)
            {
                NavigationStack.Clear();
                return;
            }

            // Set placeholder for next page
            _nextPage = target;

            // Add page to navigation stack
            NavigationStack.Add(_nextPage);

            await Task.Run(() => _nextPage?.LoadDataFromDatabase());
        }

        public static void EndNavigate()
        {
            // Set current page and reset next page
            CurrentPage = _nextPage;
            _nextPage = null;

            // Invoke loading event to indicate no longer loading
            LoadingStateChanged?.Invoke(false);
        }

        public static async void GoBack()
        {
            // Navigation failed
            if (!NavigationStack.Any()) return;

            NavigationStack.Remove(NavigationStack.GetEnumerator().Current);
            await BeginNavigate(NavigationStack.GetEnumerator().Current);
            EndNavigate();
        }

        public static async Task<bool> ConfirmDiscardChanges(IDialogCoordinator dialogCoordinator)
        {
            var result = await dialogCoordinator.ShowMessageAsync(CurrentPage,
                    "Bekræft navigation",
                    "Du har ugemte ændringer, ønsker du at forlade siden?",
                    MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Forlad siden",
                        NegativeButtonText = "Fortryd"
                    });

            return result == MessageDialogResult.Affirmative;
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
        private static ViewModelBase _nextPage;
    }
}
