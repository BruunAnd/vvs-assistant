using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Database;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class ExistingPackagedSolutionsViewModel : ViewModelBase
    {
        private ObservableCollection<PackagedSolutionViewModel> _packagedSolutions = new ObservableCollection<PackagedSolutionViewModel>();
        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions
        {
            get { return _packagedSolutions; }
        }

        public FilterableListViewModel<PackagedSolutionViewModel> FilterablePackagedSolutionsList { get; private set; }

        public ExistingPackagedSolutionsViewModel()
        {
            // Load list of packaged solutions from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of PackagedSolution to a list of PackagedSolutionViewModel
                dbContext.PackagedSolutions.ToList().ForEach(p => PackagedSolutions.Add(new PackagedSolutionViewModel(p)));
                // Create filterable list
                FilterablePackagedSolutionsList = new FilterableListViewModel<PackagedSolutionViewModel>(PackagedSolutions);
            }
        }
    }
}
