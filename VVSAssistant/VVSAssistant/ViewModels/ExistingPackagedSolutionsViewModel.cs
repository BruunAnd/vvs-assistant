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
using VVSAssistant.Extensions;
using VVSAssistant.Models;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    internal class ExistingPackagedSolutionsViewModel : FilterableViewModelBase<PackagedSolution>
    {
        public ObservableCollection<PackagedSolution> PackagedSolutions { get; set; }

        public ExistingPackagedSolutionsViewModel()
        {
            SetupFilterableView(PackagedSolutions);
        }

        public override void Initialize()
        {
            // Load list of packaged solutions from database
            DbContext.PackagedSolutions.ToList().ForEach(PackagedSolutions.Add);
        }

        protected override bool Filter(PackagedSolution obj)
        {
            return obj.Name.ContainsIgnoreCase(FilterString) || obj.Description.ContainsIgnoreCase(FilterString);
        }
    }
}
