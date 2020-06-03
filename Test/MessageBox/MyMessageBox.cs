using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BodorThinker2000.View.Dialog
{
    /// <summary>
    /// BodorThinkerMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class BodorThinkerMessageBox : Window
    {
        public BodorThinkerMessageBox(string msg, MessageType _type = MessageType.Default, MessageBoxButtons _buttontype = MessageBoxButtons.Confirm)
        {
            InitializeComponent();
            Message.Content = msg;
            this.Owner = Application.Current.MainWindow;
            if (_buttontype == MessageBoxButtons.Confirm)
                Grid0.Visibility = Visibility.Visible;
            else if (_buttontype == MessageBoxButtons.ConfirmOrCancel)
                Grid1.Visibility = Visibility.Visible;
            else if (_buttontype == MessageBoxButtons.YesOrNo)
                Grid2.Visibility = Visibility.Visible;
            else if (_buttontype == MessageBoxButtons.YesOrNoOrCancel)
                Grid3.Visibility = Visibility.Visible;
            switch (_type)
            {
                case MessageType.Default:
                    break;
                case MessageType.Info:
                    Icon.Fill = Application.Current.Resources["MessageBoxInfoIcon"] as Brush;
                    break;
                case MessageType.Warning:
                    Icon.Fill = Application.Current.Resources["MessageBoxWarnIcon"] as Brush;
                    break;
                case MessageType.Error:
                    Icon.Fill = Application.Current.Resources["MessageBoxErrorIcon"] as Brush;
                    break;
                default:
                    break;
            }
        }

        public enum MessageType
        {
            Default = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }

        public enum MessageBoxButtons
        {
            Confirm = 0,
            ConfirmOrCancel = 1,
            YesOrNo = 2,
            YesOrNoOrCancel = 3
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            this.Owner.Activate();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
            this.Owner.Activate();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            this.Owner.Activate();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
            this.Owner.Activate();
        }
    }
}
