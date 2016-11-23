using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVSAssistant.Models
{
    public class ApplianceList : IList<Appliance>
    {
        public List<ApplianceInstance> BackingList { get; }

        public ApplianceList(List<ApplianceInstance> backingList)
        {
            BackingList = backingList;
        }

        public ApplianceList(List<Appliance> applianceList)
        {
            // This might not works, depends on whether it changes the original reference.. todo
            BackingList = new List<ApplianceInstance>(applianceList.Count);
            applianceList.ForEach(Add);
        }

        public Appliance this[int index]
        {
            get { return BackingList[index].Appliance; }
            set { BackingList[index] = new ApplianceInstance(value); }
        }

        public int Count => BackingList.Count;

        public bool IsReadOnly => false;

        public void Add(Appliance item)
        {
            BackingList.Add(new ApplianceInstance(item));
        }

        public void Clear()
        {
            BackingList.Clear();
        }

        public bool Contains(Appliance item)
        {
            return BackingList.Any(x => x.Appliance == item);
        }

        public void CopyTo(Appliance[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Appliance> GetEnumerator()
        {
            return BackingList.Select(x => x.Appliance).GetEnumerator();
        }

        public int IndexOf(Appliance item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Appliance item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Appliance item)
        {
            return BackingList.RemoveAll(x => x.Appliance == item) > 0;
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
