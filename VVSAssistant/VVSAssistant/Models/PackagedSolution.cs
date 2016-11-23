using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VVSAssistant.Models
{
    public class PackagedSolution
    {
        public PackagedSolution()
        {
            ApplianceInstances = new List<ApplianceInstance>();
        }

        [NotMapped]
        public Appliance SolarContainer
        {
            get { return SolarContainerInstance.Appliance; }    
            set { SolarContainerInstance = new ApplianceInstance(value);}
        }

        [NotMapped]
        public Appliance PrimaryHeatingUnit
        {
            get { return PrimaryHeatingUnitInstance.Appliance; }
            set { PrimaryHeatingUnitInstance = new ApplianceInstance(value); }
        }

        private ApplianceList _applianceList;
        [NotMapped]
        public ApplianceList Appliances
        {
            get { return _applianceList ?? (_applianceList = new ApplianceList((List<ApplianceInstance>) ApplianceInstances)); }
            set
            {
                _applianceList = value;
                ApplianceInstances = _applianceList.BackingList;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ApplianceInstance> ApplianceInstances { get; set; }
        public virtual ApplianceInstance SolarContainerInstance { get; set; }
        public virtual ApplianceInstance PrimaryHeatingUnitInstance { get; set; }
        public string Description => "todo";// string.Join(", ", Appliances);
    }

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
