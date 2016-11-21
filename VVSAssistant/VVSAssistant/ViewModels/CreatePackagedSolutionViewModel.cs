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
        private PackagedSolutionsViewModel _packageSolution = new PackagedSolutionsViewModel();
        public PackagedSolutionsViewModel PackageSolution
        {
            get { return _packageSolution; }
        }

        private ObservableCollection<ApplianceViewModel> _appliances = new ObservableCollection<ApplianceViewModel>();
        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get { return _appliances; }
        }

        #endregion

        public CreatePackagedSolutionViewModel()
        {
            // Load list of appliances from database
            using (var dbContext = new AssistantContext())
            {
                var applianceList = dbContext.Appliances.ToList();
                applianceList.ForEach(a => _appliances.Add(new ApplianceViewModel(a)));
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
