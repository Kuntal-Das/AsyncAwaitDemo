using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncAwaitPrc.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private Func<object, Task> executeAsync;
        private Func<object, bool> canExecute;

        public AsyncRelayCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await executeAsync(parameter);
            RaiseCanExecuteChanged();
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
