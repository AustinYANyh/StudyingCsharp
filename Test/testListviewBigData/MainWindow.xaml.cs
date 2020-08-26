using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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

namespace testListView1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string normalLogFileName = "20200818.Normal.txt";

            List<string> dataList = new List<string>();
            List<Paragraph> list = new List<Paragraph>();
            //插入数据
            DataTable table = new DataTable();
            //时间、种类、信息列
            DataColumn timeCol = new DataColumn("Time", typeof(string));
            DataColumn typeCol = new DataColumn("Type", typeof(string));
            DataColumn mesgCol = new DataColumn("Mesg", typeof(string));
            table.Columns.Add(timeCol);
            table.Columns.Add(typeCol);
            table.Columns.Add(mesgCol);
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(LogFilePath, normalLogFileName), Encoding.UTF8))
            {
                string time = string.Empty;
                string type = string.Empty;
                string mesg = string.Empty;

                while (!sr.EndOfStream)
                {
                    try
                    {
                        string text = sr.ReadLine();
                        string[] str = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        if (str.Count() > 4)
                        {
                            time = string.Format("{0} {1}", str[0], str[1]);
                            type = str[2];
                            mesg = string.Format("{0} {1}", str[3], str[4]);
                        }
                        else
                        {
                            time = string.Format("{0} {1}", str[0], str[1]);
                            type = str[2];
                            mesg = str[3];
                        }
                        //dataList.Add(text);
                        DataRow row = table.NewRow();
                        row[0] = time;
                        row[1] = type;
                        row[2] = mesg;
                        //if (!table.Rows.Contains(text))
                        table.Rows.Add(row);
                    }
                    catch { }
                }
                sr.Close();
            }
            GridView gv = new GridView();
            gv.ColumnHeaderContainerStyle = (Style)this.FindResource("myHeaderStyle");
            foreach (DataColumn dc in table.Columns)
            {
                GridViewColumn columnRowNum = new GridViewColumn();
                //columnRowNum.DisplayMemberBinding = new Binding(dc.ColumnName);
                columnRowNum.Header = dc.ColumnName;
                if (dc.ColumnName == "Time")
                    columnRowNum.CellTemplate = (DataTemplate)this.FindResource("myTimeCellStyle");
                else if (dc.ColumnName == "Type")
                    columnRowNum.CellTemplate = (DataTemplate)this.FindResource("myTypeCellStyle");
                else
                    columnRowNum.CellTemplate = (DataTemplate)this.FindResource("myMesgCellStyle");
                gv.Columns.Add(columnRowNum);
            }
            Loginfo.View = gv;
            Loginfo.DataContext = table;
            Loginfo.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }
        public string LogFilePath { get { return @"D:\bdData\logs"; } }
    }

    public class ColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "普通")
                return Brushes.Black;
            else if (value.ToString() == "流程")
                return Brushes.Orange;
            else if (value.ToString() == "报警信息")
                return Brushes.Red;
            else if (value.ToString() == "坐标相关")
                return Brushes.Aqua;
            else
                return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
