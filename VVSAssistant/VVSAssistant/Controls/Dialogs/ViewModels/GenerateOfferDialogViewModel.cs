using System;
using System.Threading;
using VVSAssistant.ViewModels.MVVM;
using MahApps.Metro.Controls.Dialogs;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class GenerateOfferDialogViewModel : ViewModelBase
    {
        private OfferViewModel _offer;

        public GenerateOfferDialogViewModel(OfferViewModel offer, IDialogCoordinator dialogCoordinator, Action<GenerateOfferDialogViewModel> closeHandler)
        {
            _offer = offer;
        }
    }
}

