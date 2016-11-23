using System.Collections.ObjectModel;
using System.Linq;
using VVSAssistant.Common.ViewModels.VVSAssistant.Common.ViewModels;
using VVSAssistant.Extensions;
using VVSAssistant.Models;

namespace VVSAssistant.ViewModels
{
    internal class ExistingPackagedSolutionsViewModel : FilterableViewModelBase<PackagedSolution>
    {
        public ObservableCollection<PackagedSolution> PackagedSolutions { get; } = new ObservableCollection<PackagedSolution>();

        public ExistingPackagedSolutionsViewModel()
        {
            SetupFilterableView(PackagedSolutions);
        }

        public override void LoadDataFromDatabase()
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
