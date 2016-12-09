using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using VVSAssistant.Common;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Controls.Dialogs.ViewModels
{
    class TimedMessageViewModel : NotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Message { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        RelayCommand CloseHandler;

        public TimedMessageViewModel(string title, string message, double time, Action<TimedMessageViewModel> closeHandler)
        {
            Title = title;
            Message = message;
            CloseHandler = new RelayCommand(x => closeHandler(this));
            timer.Interval = TimeSpan.FromSeconds(time); //Ticks after two seconds
            timer.Tick += OnTick; //When tick happens, run this method
            timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            timer.Stop();
            CloseHandler.Execute(this);
        }
    }
}
