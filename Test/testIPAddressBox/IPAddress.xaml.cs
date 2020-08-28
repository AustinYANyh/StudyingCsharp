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

namespace testIPAddressBox
{
    /// <summary>
    /// IPAddress.xaml 的交互逻辑
    /// </summary>
    public partial class IPAddress : UserControl
    {
        public IPAddressViewModel viewModel;
        public IPAddress()
        {
            InitializeComponent();
            viewModel = new IPAddressViewModel();
            this.DataContext = viewModel;
        }

        private void Part1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back) ;

            if (e.Key == Key.Right && part1.CaretIndex == part1.Text.Length)
            {
                part2.Focus();
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part1.SelectionLength == 0)
                {
                    var vm = this.DataContext as IPAddressViewModel;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && part2.Text == string.Empty)
            {
                part1.Focus();
                part1.CaretIndex = part1.Text.Length;
            }
            if (e.Key == Key.Right && part2.CaretIndex == part2.Text.Length)
            {
                part3.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Left && part2.CaretIndex == 0)
            {
                part1.Focus();
                part1.CaretIndex = part1.Text.Length;
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part2.SelectionLength == 0)
                {
                    var vm = this.DataContext as IPAddressViewModel;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && part3.Text == string.Empty)
            {
                part2.Focus();
                part2.CaretIndex = part2.Text.Length;
            }
            if (e.Key == Key.Right && part3.CaretIndex == part3.Text.Length)
            {
                part4.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Left && part3.CaretIndex == 0)
            {
                part2.Focus();
                part2.CaretIndex = part2.Text.Length;
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part3.SelectionLength == 0)
                {
                    var vm = this.DataContext as IPAddressViewModel;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }

        private void Part4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && part4.Text == string.Empty)
            {
                part3.Focus();
                part3.CaretIndex = part3.Text.Length;
            }
            if (e.Key == Key.Right && part4.CaretIndex == part4.Text.Length)
                e.Handled = true;
            if (e.Key == Key.Left && part4.CaretIndex == 0)
            {
                part3.Focus();
                part3.CaretIndex = part3.Text.Length;
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                if (part4.SelectionLength == 0)
                {
                    var vm = this.DataContext as IPAddressViewModel;
                    Clipboard.SetText(vm.AddressText);
                }
            }
        }//end for func
    }

    //输入规则
    public class IPRangeValidationRule : ValidationRule
    {
        private int _min;
        private int _max;

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;
            var strVal = (string)value;
            try
            {
                if (strVal.Length > 0)
                {
                    if (strVal.EndsWith("."))
                    {
                        return CheckRanges(strVal.Replace(".", ""));
                    }

                    // Allow dot character to move to next box
                    return CheckRanges(strVal);
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                 "Please enter the value in the range: " + Min + " - " + Max + ".");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

        private ValidationResult CheckRanges(string strVal)
        {
            if (int.TryParse(strVal, out var res))
            {
                if ((res < Min) || (res > Max))
                {
                    return new ValidationResult(false,
                     "Please enter the value in the range: " + Min + " - " + Max + ".");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            else
            {
                return new ValidationResult(false, "Illegal characters entered");
            }
        }
    }//end for valuerules

    public class FocusChangeExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
          "IsFocused", typeof(bool), typeof(FocusChangeExtension), new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UIElement)d;
            if ((bool)e.NewValue)
                control.Focus();
        }
    }//end for focuscontrol
}
