using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VVSAssistant.Common
{
    /* The difference between this class and its parent, ObservableCollection,
     * is that elements in the collection can be added and removed from
     * different threads. */
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly object _collectionLock;

        public AsyncObservableCollection()
        {
            _collectionLock = new object();
            BindingOperations.EnableCollectionSynchronization(this, _collectionLock);
        }
    }
}
