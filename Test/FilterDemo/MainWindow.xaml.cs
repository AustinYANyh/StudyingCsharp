using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FilterDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;

            ObservableCollection<ProdPrepareDrawingData> listData = new ObservableCollection<ProdPrepareDrawingData>();
            for (int i = 1; i < 20; i++)
            {
                ProdPrepareDrawingData data = new ProdPrepareDrawingData();
                data.Index = i;
                data.DrawingName = "图纸" + i;
                data.TaskName = "任务" + i;
                data.ProcessName = "工艺" + i;
                data.ProductionCount = "5";
                data.ProdcutionStatus = "测试";
                data.DrawingSize = (i * 100).ToString() + " * 200.000";
                data.Sort = "Y";
                listData.Add(data);
            }
            viewModel.ProdPrepareDrawingDataList = listData;

            if (viewModel.ProdPrepareDrawingDataList.Count != 0)
            {
                viewModel.SelectedProdPrepareDrawingData = viewModel.ProdPrepareDrawingDataList[0];
            }

            foreach (var each in viewModel.ProdPrepareDrawingDataList)
            {
                tmp.Add(each);
            }
        }

        private ObservableCollection<ProdPrepareDrawingData> tmp = new ObservableCollection<ProdPrepareDrawingData>();

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (viewModel.SelectedProdPrepareDrawingData == null)
            {
                return;
            }

            if (e.Delta < 0)
            {
                if (dataGrid.SelectedIndex != dataGrid.Items.Count - 1)
                {
                    dataGrid.SelectedIndex += 1;
                }
            }
            else
            {
                if (dataGrid.SelectedIndex != 0)
                {
                    dataGrid.SelectedIndex -= 1;
                }
            }
        }

        private void listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ProdPrepareDrawingDataList = tmp;
            ObservableCollection<ProdPrepareDrawingData> list = new ObservableCollection<ProdPrepareDrawingData>();
            foreach(var each in viewModel.ProdPrepareDrawingDataList)
            {
                if(each.Index >10 )
                {
                    list.Add(each);
                }
            }
            viewModel.ProdPrepareDrawingDataList = list;
        }

        private void listbox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ProdPrepareDrawingDataList = tmp;
            ObservableCollection<ProdPrepareDrawingData> list = new ObservableCollection<ProdPrepareDrawingData>();
            foreach (var each in viewModel.ProdPrepareDrawingDataList)
            {
                if (each.Index < 10)
                {
                    list.Add(each);
                }
            }
            viewModel.ProdPrepareDrawingDataList = list;
        }
    }
}
