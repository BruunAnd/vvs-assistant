//using System;
//using System.Threading.Tasks;
//using MahApps.Metro.Controls;
//using System.Windows;
//using MahApps.Metro.Controls.Dialogs;

//namespace VVSAssistant.ViewModels.MVVM
//{
//    public static class DialogCoordinator
//    {
//        public static Task<string> ShowDialog(object context, string title, string message)
//        {
//            if (context == null) throw new ArgumentNullException(nameof(context));
//            if (!DialogParticipation.IsRegistered(context))
//                throw new InvalidOperationException(
//                    "Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");

//            var association = DialogParticipation.GetAssociation(context);
//            var metroWindow = Window.GetWindow(association) as MetroWindow;

//            if (metroWindow == null)
//                throw new InvalidOperationException("Control is not inside a MetroWindow.");

//            return metroWindow.ShowInputAsync(title, message);
//        }
//    }
//}
