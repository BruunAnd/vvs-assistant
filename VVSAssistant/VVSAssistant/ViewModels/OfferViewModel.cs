﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Models;
using VVSAssistant.ViewModels.MVVM;
using VVSAssistant.Exceptions;
using System.Collections.ObjectModel;

namespace VVSAssistant.ViewModels
{
    class OfferViewModel : ViewModelBase
    {
        private readonly Offer _offer;
        private PackagedSolutionViewModel _packagedSolution;
        private OfferInformationViewModel _offerInformation;
        private ClientViewModel _client;
        private ObservableCollection<MaterialViewModel> _materials;
        private ObservableCollection<SalaryViewModel> _salaries;

        public OfferViewModel(Offer offer)
        {
            _offer = offer;
            _client = new ClientViewModel(offer.Client);
            _packagedSolution = new PackagedSolutionViewModel(offer.PackagedSolution);
            _offerInformation = new OfferInformationViewModel(offer.OfferInformation);

            _materials = new ObservableCollection<MaterialViewModel>();
            _salaries = new ObservableCollection<SalaryViewModel>();

            foreach (var material in offer.Materials)
                _materials.Add(new MaterialViewModel(material));
            foreach (var salary in offer.Salaries)
                _salaries.Add(new SalaryViewModel(salary));
        }

        public PackagedSolutionViewModel PackagedSolution
        {
            get { return _packagedSolution; }
            set
            {
                _packagedSolution = value;
                OnPropertyChanged();
            }
        }

        public OfferInformationViewModel OfferInformation
        {
            get { return _offerInformation; }
            set
            {
                _offerInformation = value;
                OnPropertyChanged();
            }
        }

        public ClientViewModel Client
        {
            get { return _client; }
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get { return _offer.Id; }
            set
            {
                _offer.Id = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreationDate
        {
            get { return _offer.CreationDate; }
            set
            {
                /* Can only set creation date once */
                if (_offer.CreationDate != null)
                {
                    _offer.CreationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MaterialViewModel> Materials
        {
            get { return _materials; }
            set
            {
                _materials = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SalaryViewModel> Salaries
        {
            get { return _salaries; }
            set
            {
                _salaries = value;
                OnPropertyChanged();
            }
        }
    }
}
