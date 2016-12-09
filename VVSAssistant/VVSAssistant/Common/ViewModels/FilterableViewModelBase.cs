using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace VVSAssistant.Common.ViewModels
{
    public abstract class FilterableViewModelBase<T> : ViewModelBase
    {
        public ICollectionView FilteredCollectionView { get; private set; }

        private string _filterString = "";
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                if (_filterString.Equals(value)) return;
                _filterString = value;
                FilteredCollectionView.Refresh();
                OnPropertyChanged();
            }
        }

        protected void SetupFilterableView(ICollection<T> dataSource)
        {
            FilteredCollectionView = CollectionViewSource.GetDefaultView(dataSource);
            FilteredCollectionView.Filter = obj => Filter((T)obj);
        }

        protected abstract bool Filter(T obj);
    }
}