using System;
using System.Windows.Input;

namespace VVSAssistant.Common.ViewModels
{
    /// <summary>
    /// This is what commands in the View are bound to.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecutePredicate;

        private readonly Action<object> _executeAction;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate = null)
        {
            this._executeAction = executeAction;
            this._canExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter) => this._canExecutePredicate?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this._executeAction?.Invoke(parameter);

        public void NotifyCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        
    }
}
