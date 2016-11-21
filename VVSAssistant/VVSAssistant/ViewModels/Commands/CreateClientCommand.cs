using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVSAssistant.ViewModels.MVVM;

namespace VVSAssistant.ViewModels.Commands
{
    public class CreateClientCommand : RelayCommand
    {
        public CreateClientCommand(Action<object> executeAction, Predicate<object> canExecutePredicate = null) 
            : base(executeAction, canExecutePredicate)
        {
        }
    }
}
