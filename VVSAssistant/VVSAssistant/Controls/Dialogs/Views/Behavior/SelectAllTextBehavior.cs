using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace VVSAssistant.Controls.Dialogs.Views.Behavior
{
    public class SelectAllTextBehavior :Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.GotFocus += this.OnTextBoxGotFocus;
            this.AssociatedObject.GotMouseCapture += this.OnTextBoxGotFocus;
            base.OnAttached();
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.GotFocus -= this.OnTextBoxGotFocus;
            this.AssociatedObject.GotMouseCapture -= this.OnTextBoxGotFocus;
            base.OnDetaching();
        }
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.SelectAll();
        }
    }
}
