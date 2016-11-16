using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VVSAssistant.View_Models
{
    /// <summary>
    /// Base class for ViewModel classes.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
