using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VVSAssistant.ViewModels
{
    /// <summary>
    /// Base class for ViewModel classes.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
