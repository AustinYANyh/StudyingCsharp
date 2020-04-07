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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BodorLaserWPF.Schedule.View
{
    /// <summary>
    /// DayList.xaml 的交互逻辑
    /// </summary>
    public partial class DayList : UserControl
    {
        public DayList()
        {
            InitializeComponent();
            this.Loaded += DayList_Loaded;
        }

        private void DayList_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 31; ++i)
            {
                DataSourceDay.Add(i);
            }

            startIndex = DataSourceDay.IndexOf(Convert.ToInt32(DateTime.Now.ToString("dd"))) - 2;
            RefreshData();
        }

        public List<int> DataSourceDay
        {
            get { return (List<int>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSourceDay", typeof(List<int>), typeof(DayList), new PropertyMetadata(new List<int>()));

        public int SelectData
        {
            get { return (int)GetValue(SelectDataProperty); }
            set { SetValue(SelectDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectDataProperty =
            DependencyProperty.Register("SelectData", typeof(int), typeof(DayList), new PropertyMetadata(0));



        public int ItemHeight
        {
            get { return (int)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(int), typeof(DayList), new PropertyMetadata(75));

        int ShowItemCount
        {
            get
            {
                return (int)ActualHeight / ItemHeight;
            }
        }

        int startIndex = 0;


        private void tt_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine("TimeStap={0} Delta={1}", e.Timestamp, e.Delta);

            if (DataSourceDay.Count == 0)
                return;

            if (e.Delta < 0)
            {
                if ((startIndex + ShowItemCount) < DataSourceDay.Count)
                {
                    startIndex += 1;
                }
                else
                {
                    startIndex = 0;
                }
            }
            else
            {
                if ((startIndex - ShowItemCount) < 0)
                {
                    startIndex = DataSourceDay.Count - 1;
                }
                else
                {
                    startIndex -= 1;
                }

            }

            RefreshData();

        }

        private void RefreshData()
        {
            if (DataSourceDay.Count > 0)
            {
                int count = 0;
                foreach (var item in stacktt.Children)
                {
                    if ((startIndex + count) > (DataSourceDay.Count - 1))
                    {
                        (item as TextBlock).Text = DataSourceDay[startIndex + count - DataSourceDay.Count].ToString();
                    }
                    else
                    {
                        (item as TextBlock).Text = DataSourceDay[startIndex + count].ToString();
                    }
                    count += 1;
                }
                TextBlock selectText = (TextBlock)VisualTreeHelper.GetChild(stacktt, ShowItemCount / 2);

                if (ShowItemCount % 2 != 0)
                {
                    selectText = (TextBlock)VisualTreeHelper.GetChild(stacktt, ShowItemCount / 2);
                }
                SelectData = Convert.ToInt32(selectText.Text);
            }
        }
    }
}
