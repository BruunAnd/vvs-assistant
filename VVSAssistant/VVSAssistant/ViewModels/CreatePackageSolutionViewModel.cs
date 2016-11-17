using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace VVSAssistant.ViewModels
{
    internal class CreatePackageSolutionViewModel
    {
        #region Commands
        public RelayCommand AddApplianceToPackageSolution { get; }
        public RelayCommand RemoveApplianceFromPackageSolution { get; }
        public RelayCommand EditAppliance { get; }
        public RelayCommand RemoveAppliance { get; }
        public RelayCommand NewPackageSolution { get; }
        
        #endregion

        public CreatePackageSolutionViewModel()
        {
            PackageSolution = new PackageSolutionViewModel();
            Appliances = new ObservableCollection<ApplianceViewModel>();
            
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
            
        }

        private void PackageSolutionAppliances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NewPackageSolution.NotifyCanExecuteChanged();
        }

        public PackageSolutionViewModel PackageSolution
        {
            get; private set;
        }
        
        public ObservableCollection<ApplianceViewModel> Appliances
        {
            get; private set;
        }
    }
}
