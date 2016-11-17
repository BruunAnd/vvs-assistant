using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VVSAssistant.ViewModels
{
    /// <summary>
    /// This is what commands in the View are bound to.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecutePredicate;

        private readonly Action<object> executeAction;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate = null)
        {
            this.executeAction = executeAction;
            this.canExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter) => this.canExecutePredicate?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.executeAction?.Invoke(parameter);

        public void NotifyCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
