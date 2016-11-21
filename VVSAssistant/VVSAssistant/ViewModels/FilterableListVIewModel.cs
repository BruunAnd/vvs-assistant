using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VVSAssistant.ViewModels.Interfaces;

namespace VVSAssistant.ViewModels
{
    class FilterableListViewModel<T> where T : IFilterable
    {
        public ICollectionView FilteredView { get; private set; }

        public string FilterString { get; private set; }

        public FilterableListViewModel(List<T> dataSource)
        {
            FilteredView = CollectionViewSource.GetDefaultView(dataSource);
        }
    }
}
