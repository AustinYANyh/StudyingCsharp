using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace caltextBox
{
    /// <summary>
    /// calTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class calTextBox : UserControl
    {
        public calTextBox()
        {
            InitializeComponent();
        }

        private void StayTimebtn_Click(object sender, RoutedEventArgs e)
        {
            if (myPop.IsOpen == false)
            {
                myPop.PlacementTarget = StayTimebtn;
                myPop.Placement = PlacementMode.Bottom;
                myPop.HorizontalOffset = StayTimebtn.Width - myPop.Width;
                myPop.IsOpen = true;
            }
            else
            {
                myPop.IsOpen = false;
            }

        }

        private bool bRadixPointFlag = false; //小数点标志
        private int ClickCount = 1;

        private void btnNumerButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if(ClickCount == 1)
            {
                this.txtShowNum.Text = "";
            }
            ClickCount += 1;
            //有几种情况
            if (bRadixPointFlag)
            {
                this.txtShowNum.Text += button.Content.ToString();
            }
            else
            {
                if (this.txtShowNum.Text == "0.")
                {
                    if (button.Content.ToString() == "0")
                    {
                        return;
                    }
                    else
                    {
                        this.txtShowNum.Text = button.Content.ToString() + ".";
                    }
                }
                else
                {
                    this.txtShowNum.Text = this.txtShowNum.Text.Replace('.', ' ').Trim();
                    this.txtShowNum.Text += button.Content.ToString() + ".";
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            bRadixPointFlag = false;
            this.txtShowNum.Text = "0.";
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (bRadixPointFlag)
            {
                return;
            }
            bRadixPointFlag = true;
        }

        private void btnSymbol_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtShowNum.Text.Contains("-"))
            {
                this.txtShowNum.Text.Remove(0, 1); //负号总在最前面
            }
            else
            {
                this.txtShowNum.Text.Insert(0, "-");
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //判断最后一位是带小数点，若带小数点则为正数
            int index = this.txtShowNum.Text.Trim().LastIndexOf('.');
            if (index == this.txtShowNum.Text.Length - 1)
            {
                ResultBox.Text = this.txtShowNum.Text.Trim().Replace(".", "");
                myPop.IsOpen = false;
            }
            else
            {
                //RecvData(sender, this.txtShowNum.Text.Trim());
                ResultBox.Text = this.txtShowNum.Text.Trim().Replace(".","");
                myPop.IsOpen = false;
            }
            ClickCount = 1;
        }

        public static readonly DependencyProperty MyPropertyText = DependencyProperty.Register("MyProperty", typeof(string), typeof(calTextBox),
                                                                new PropertyMetadata(defaultValue: "0."));

        public string MyProperty
        {
            get
            {
                return (string)GetValue(MyPropertyText);
            }
            set
            {
                SetValue(MyPropertyText, value);
                if (value.ToString().Contains("."))
                {
                    this.txtShowNum.Text = value.ToString();
                }
                else
                {
                    this.txtShowNum.Text = value.ToString() + ".";
                }

            }
        }
    }

    public class YourConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
