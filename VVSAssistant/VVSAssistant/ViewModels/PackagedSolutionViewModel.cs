﻿using System;
using System.Collections.ObjectModel;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.Interfaces;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels
{
    class PackagedSolutionViewModel : ViewModelBase, IFilterable
    {
        // Base model
        private PackagedSolution _packagedSolution;

        public ObservableCollection<ApplianceViewModel> Appliances { get; private set; }

        public PackagedSolutionViewModel() : this(new PackagedSolution()) { }

        public PackagedSolutionViewModel(PackagedSolution packagedSolution)
        {
            _packagedSolution = packagedSolution;

            // Transform list of Appliance to list of ApplianceViewModel
            Appliances = new ObservableCollection<ApplianceViewModel>();
            foreach (var appliance in _packagedSolution.Appliances)
            {
                Appliances.Add(new ApplianceViewModel(appliance));
            }
        }

        public string Name
        {
            get { return _packagedSolution.Name; }
            set
            {
                if (_packagedSolution.Name == value) return;
                _packagedSolution.Name = value;
                OnPropertyChanged();
            }
        }

        public string CreationDate => _packagedSolution.CreationDate.ToString(@"dd\/MM\/yyyy HH:mm");

        public string Description => string.Join(" ", Appliances);

        public bool DoesFilterMatch(string query)
        {
            foreach (IFilterable appliance in Appliances)
            {
                if (appliance.DoesFilterMatch(query))
                    return true;
            }
            return false;
        }
    }
}
