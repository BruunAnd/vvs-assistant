using System;
using System.Collections.Generic;
using VVSAssistant.Common;

namespace VVSAssistant.Models
{
    public class OfferInformation : NotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _intro;
        public string Intro
        {
            get { return _intro; }
            set { SetProperty<string>(ref _intro, value); }
        }

        private string _outro;
        public string Outro
        {
            get { return _outro; }
            set { SetProperty<string>(ref _outro, value); }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty<string>(ref _title, value); }
        }
        private bool _applyTax;
        public bool ApplyTax
        {
            get { return _applyTax; }
            set { SetProperty<bool>(ref _applyTax, value); }
        }
    }
}
