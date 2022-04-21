using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AsyncAwaitPrc.ViewModel
{
    public abstract class BaseNotifyVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
