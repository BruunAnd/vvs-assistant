using VVSAssistant.ViewModels.Interfaces;

namespace VVSAssistant.Common.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;

    namespace VVSAssistant.Common.ViewModels
    {
        public abstract class FilterableViewModelBase<T> : ViewModelBase where T : IFilterable
        {
            public ICollectionView CollectionView { get; set; }

            private string _filterString = "";
            public string FilterString
            {
                get { return _filterString; }
                set
                {
                    if (_filterString.Equals(value)) return;
                    _filterString = value;
                    CollectionView.Refresh();
                    OnPropertyChanged();
                }
            }

            protected void SetupFilterableView(ICollection<T> dataSource)
            {
                CollectionView = CollectionViewSource.GetDefaultView(dataSource);
                CollectionView.Filter = obj => Filter((T)obj);
            }

            private bool Filter(IFilterable filterObj)
            {
                return filterObj.DoesFilterMatch(FilterString);
            }
        }
    }

}
