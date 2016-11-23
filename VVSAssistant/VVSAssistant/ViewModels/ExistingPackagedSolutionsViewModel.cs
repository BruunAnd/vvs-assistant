using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.Database;
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.Interfaces;

namespace VVSAssistant.ViewModels
{
    internal class ExistingPackagedSolutionsViewModel : FilterableViewModelBase<PackagedSolution>
    {
        public ObservableCollection<PackagedSolution> PackagedSolutions { get; }

        protected override bool Filter(PackagedSolution filterObj)
        {
            return filterObj.Name.ContainsIgnoreCase(FilterString) || filterObj.Description.ContainsIgnoreCase(FilterString);
        }

        public ExistingPackagedSolutionsViewModel()
        {
            PackagedSolutions = new ObservableCollection<PackagedSolution>();
            SetupFilterableView(PackagedSolutions);

            // Load list of packaged solutions from database
            using (var dbContext = new AssistantContext())
            {
                // Transform list of PackagedSolution to a list of PackagedSolutionViewModel
                dbContext.PackagedSolutions.ToList().ForEach(x => PackagedSolutions.Add(x));
            }
        }
    }
}
