using System;
using System.Collections.Generic;
using System.Data;
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
            DataTable table = new DataTable();
            table.Columns.Add("rizhi");
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(LogFilePath, normalLogFileName), Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    string text = sr.ReadLine();
                    dataList.Add(text);
                    DataRow row = table.NewRow();
                    row[0] = text;
                    //if (!table.Rows.Contains(text))
                    table.Rows.Add(row);
                }
                sr.Close();
            }
            GridView gv = new GridView();
            //插入数据
            DataTable dtRowNumber = new DataTable();
            DataColumn dcRowNum = new DataColumn("RowNum", typeof(string));
            dtRowNumber.Columns.Add(dcRowNum);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = dtRowNumber.NewRow();
                dr[0] = dataList[i].ToString();
                dtRowNumber.Rows.Add(dr);
            }

            GridViewColumn columnRowNum = new GridViewColumn();
            foreach (DataColumn dc in dtRowNumber.Columns)
            {
                columnRowNum.DisplayMemberBinding = new Binding(dc.ColumnName);
                columnRowNum.Header = dc.ColumnName;
                gv.Columns.Insert(0, columnRowNum);
            }
            Loginfo.View = gv;
            Loginfo.DataContext = dtRowNumber;
            Loginfo.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }
        public string LogFilePath { get { return @"D:\bdData\logs"; } }
    }
}
