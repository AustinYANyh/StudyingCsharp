using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testCombox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }
    
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; ++i)
            {
                string name = "combox" + (i + 1).ToString();
                ComboBox box = this.FindName(name) as ComboBox;
                box.DropDownOpened += Box_DropDownOpened;
            }
        }

        private void Box_DropDownOpened(object sender, EventArgs e)
        {
            for (int i = 0; i < viewModel.NowIOList.Count; ++i)
                viewModel.NowIOList[i].CanSelected = true;
            for (int i = 0; i < 3; ++i)
            {
                string name = "combox" + (i + 1).ToString();
                ComboBox box = this.FindName(name) as ComboBox;
                if (box.SelectedItem == null)
                    continue;
                viewModel.NowIOList.Find((a) => a.Index == (box.SelectedItem as IOData).Index).CanSelected = false;
            }
        }
    }
    public class YourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Visibility.Visible == (Visibility)value)
                return true;
            else
                return false;
        }
    }
}
