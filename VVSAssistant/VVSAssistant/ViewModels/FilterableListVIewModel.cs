using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VVSAssistant.Common.ViewModels;
using VVSAssistant.ViewModels.Interfaces;

namespace VVSAssistant.ViewModels
{
    public class FilterableListViewModel<T> : ViewModelBase where T : IFilterable
    {
        public ICollectionView Collection { get; }

        private string _filterString = "";
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                if (_filterString.Equals(value)) return;
                _filterString = value;
                Collection.Refresh();
                OnPropertyChanged();
            }
        }

        public FilterableListViewModel(ICollection<T> dataSource)
        {
            Collection = CollectionViewSource.GetDefaultView(dataSource);
            Collection.Filter = obj => (obj as IFilterable).DoesFilterMatch(FilterString);
        }
    }
}
