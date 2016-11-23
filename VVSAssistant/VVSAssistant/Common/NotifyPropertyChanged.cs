using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VVSAssistant.Common
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Sets a property if the field parameter is different from the
        /// value parmeter, returns true when a property is set and false
        /// otherwise.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value,
            [CallerMemberName] string propname = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propname);
                return true;
            }
            return false;
        }
    }
}