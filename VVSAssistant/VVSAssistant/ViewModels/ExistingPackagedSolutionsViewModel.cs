using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    internal class ExistingPackagedSolutionsViewModel : FilterableViewModelBase<PackagedSolutionViewModel>
    {
        public ObservableCollection<PackagedSolutionViewModel> PackagedSolutions { get; } = new ObservableCollection<PackagedSolutionViewModel>();

        public ExistingPackagedSolutionsViewModel()
        {
            SetupFilterableView(PackagedSolutions);

            // Load list of packaged solutions from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of PackagedSolution to a list of PackagedSolutionViewModel
                dbContext.PackagedSolutions.ToList().ForEach(x => PackagedSolutions.Add(new PackagedSolutionViewModel(x)));
            }
        }
    }
}
