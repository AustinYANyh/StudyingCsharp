using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testDependcy
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
    }

    public class TextBoxHelper
    {
        public static readonly DependencyProperty AutoSelectAllProperty =
        DependencyProperty.RegisterAttached("AutoSelectAll", typeof(bool), typeof(TextBoxHelper),
        new FrameworkPropertyMetadata((bool)false,
        new PropertyChangedCallback(OnAutoSelectAllChanged)));

        public static bool GetAutoSelectAll(TextBoxBase d)
        {
            return (bool)d.GetValue(AutoSelectAllProperty);
        }
        public static void SetAutoSelectAll(TextBoxBase d, bool value)
        {
            d.SetValue(AutoSelectAllProperty, value);
        }
        private static void OnAutoSelectAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBoxBase;
            if (textBox != null)
            {
                var flag = (bool)e.NewValue;
                if (flag)
                {
                    textBox.PreviewMouseDown += TextBox_PreviewMouseDown;
                    textBox.GotFocus += TextBoxOnGotFocus;
                    textBox.LostFocus += TextBox_LostFocus;
                }
                else
                {
                    textBox.PreviewMouseDown -= TextBox_PreviewMouseDown;
                    textBox.GotFocus -= TextBoxOnGotFocus;
                    textBox.LostFocus -= TextBox_LostFocus;
                }
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.PreviewMouseDown += TextBox_PreviewMouseDown;
            }
        }

        private static void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.Focus();
                e.Handled = true;
            }
        }

        private static void TextBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBoxBase;
            if (textBox != null)
            {
                textBox.SelectAll();
                textBox.PreviewMouseDown -= TextBox_PreviewMouseDown;
            }
        }
    }
}
