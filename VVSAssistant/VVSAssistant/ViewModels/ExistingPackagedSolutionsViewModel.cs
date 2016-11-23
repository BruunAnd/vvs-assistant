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
        }

        public override void Initialize()
        {
            // Load list of packaged solutions from database
            DbContext.PackagedSolutions.ToList().ForEach(x => PackagedSolutions.Add(new PackagedSolutionViewModel(x)));
        }
    }
}
