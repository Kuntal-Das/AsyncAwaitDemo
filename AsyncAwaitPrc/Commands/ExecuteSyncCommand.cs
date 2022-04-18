using AsyncAwaitPrc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncAwaitPrc.Commands
{
    class ExecuteSyncCommand : ICommand
    {
        //private Action<object> execute;
        //private Func<object, bool> canExecute;

        //public ExecuteSyncCommand(Action<object> execute, Func<object, bool> canExecute)
        //{
        //    this.execute = execute;
        //    this.canExecute = canExecute;
        //}
        private MainWindowViewModel _vm;

        public ExecuteSyncCommand(ref MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //_vm.

            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            _vm.StrStatus += $"Total execution Tme: {elapsedTime}{Environment.NewLine}";
        }
    }
}
