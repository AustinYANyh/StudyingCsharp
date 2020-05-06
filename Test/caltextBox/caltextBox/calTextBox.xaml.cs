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
    public partial class CalTextBox : UserControl
    {
        public CalTextBox()
        {
            InitializeComponent();
        }

        public void SetWidthAndHeight(int width, int height)
        {
            this.Width = width;
            ResultBox.Width = width - StayTimebtn.Width;
            ResultBox.Height = height;
            StayTimebtn.Margin = new Thickness(ResultBox.Width, 0, 0, 0);
        }

        private void StayTimebtn_Click(object sender, RoutedEventArgs e)
        {
            if (myPop.IsOpen == false)
            {
                myPop.PlacementTarget = StayTimebtn;
                myPop.Placement = PlacementMode.Bottom;
                myPop.HorizontalOffset = StayTimebtn.Width - myPop.Width;
                myPop.IsOpen = true;

                txtShowNum.Text = ResultBox.Text + ".";
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

            if (ClickCount == 1)
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
                this.txtShowNum.Text = txtShowNum.Text.Remove(0, 1); //负号总在最前面
            }
            else
            {
                this.txtShowNum.Text = txtShowNum.Text.Insert(0, "-");
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
                ResultBox.Text = this.txtShowNum.Text.Trim().Replace(".", "");
                myPop.IsOpen = false;
            }
            ClickCount = 1;

            //获得焦点然后失去焦点,是为了使ViewModel的值通过双向绑定改变,小键盘改变了ResultBox的值不触发双向绑定
            ResultBox.Focus();
            FocusTextBox.Focus();
        }

        public static readonly DependencyProperty MyPropertyText = DependencyProperty.Register("MyProperty", typeof(string), typeof(CalTextBox),
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

    public partial class MyTextBox : TextBox
    {
        public string MyUnit
        {
            get { return GetValue(MyUnitProperty).ToString(); }
            set { SetValue(MyUnitProperty, value); }
        }

        public static readonly DependencyProperty MyUnitProperty =
    DependencyProperty.Register("MyUnit", typeof(string), typeof(CalTextBox), new PropertyMetadata(defaultValue: ""));
    }

    public class YourConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
