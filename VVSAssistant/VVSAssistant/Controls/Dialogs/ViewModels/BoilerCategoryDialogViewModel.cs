using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.Common;
using VVSAssistant.Models;
using VVSAssistant.Models.DataSheets;
using VVSAssistant.ViewModels;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class BoilerCategoryDialogViewModel : NotifyPropertyChanged
    {
        private RelayCommand SaveCommand;
        private RelayCommand CloseCommand;
        private CreatePackagedSolutionViewModel _source;
        public CreatePackagedSolutionViewModel Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(); }
        }
        public BoilerCategoryDialogViewModel(CreatePackagedSolutionViewModel source, IDialogCoordinator dialogCoordinator,
                                            Action<BoilerCategoryDialogViewModel> closeHandler, Action<BoilerCategoryDialogViewModel> completionHandler)
        {
            SaveCommand = new RelayCommand(x =>
            {
                completionHandler(this);
            }, x => true);

            CloseCommand = new RelayCommand(x =>
            {
                closeHandler(this);
            });
        }
    }
}
