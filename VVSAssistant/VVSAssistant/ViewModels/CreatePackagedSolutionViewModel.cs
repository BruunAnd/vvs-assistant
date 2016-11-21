using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Database;
using System.Windows.Data;
using VVSAssistant.ViewModels.Interfaces;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    class CreatePackagedSolutionViewModel : ViewModelBase
    {
        #region Command initializations
        public RelayCommand AddApplianceToPackageSolution { get; }
        public RelayCommand RemoveApplianceFromPackageSolution { get; }
        public RelayCommand EditAppliance { get; }
        public RelayCommand RemoveAppliance { get; }
        public RelayCommand NewPackageSolution { get; }
        #endregion

        #region Collections
        private PackagedSolutionViewModel _packageSolution = new PackagedSolutionViewModel();
        public PackagedSolutionViewModel PackageSolution
        {
            get { return _packageSolution; }
        }

        private ObservableCollection<ApplianceViewModel> _appliances = new ObservableCollection<ApplianceViewModel>();
        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get { return _appliances; }
        }

        public ICollectionView FilteredApplianceList { get; private set; }
        #endregion

        private string _filterString = "";
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                if (_filterString.Equals(value)) return;
                _filterString = value;
                FilteredApplianceList.Refresh();
                OnPropertyChanged();
            }
        }

        public CreatePackagedSolutionViewModel()
        {
            // Load list of appliances from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of Appliance to a list of ApplianceViewModel
                dbContext.Appliances.ToList().ForEach(a => Appliances.Add(new ApplianceViewModel(a)));
                // Create filtered list
                FilteredApplianceList = CollectionViewSource.GetDefaultView(Appliances);
                FilteredApplianceList.Filter = obj =>
                {
                    return (obj as IFilterable).DoesFilterMatch(FilterString);
                };
            }

            #region Command declarations
            AddApplianceToPackageSolution = new RelayCommand(x => 
            {
                var item = x as ApplianceViewModel;
                if (item != null) this.PackageSolution.Appliances.Add(item);
            });

            RemoveApplianceFromPackageSolution = new RelayCommand(x =>
            {
                var item = x as ApplianceViewModel;
                if (item != null) this.PackageSolution.Appliances.Remove(item);
            });

            /* EditAppliance = new RelayCommand(x =>
             {
                 var item = x as ApplianceViewModel;
                 if (item != null)
             });
             */

            RemoveAppliance = new RelayCommand(x =>
            {
                var item = x as ApplianceViewModel;
                if (item != null)
                {
                    Appliances.Remove(item);                       
                }
            });

            NewPackageSolution = new RelayCommand(x =>
            {
                if (this.PackageSolution.Appliances.Any()) this.PackageSolution.Appliances.Clear();
            }, x => this.PackageSolution.Appliances.Any());
            this.PackageSolution.Appliances.CollectionChanged += PackageSolutionAppliances_CollectionChanged;

            #endregion
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackageSolution.NotifyCanExecuteChanged();
        }
    }
}
