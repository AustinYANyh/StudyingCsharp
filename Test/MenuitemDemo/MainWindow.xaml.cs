using LogClassHelper;
using System;
using System.Collections.Generic;
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
using System.Xml;
using Menu = LogClassHelper.Menu;

namespace MenuItem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lll = GetLogConfig();
            string xml = XmlHelper.XMLHelper.WriteXml<Menu>(lll);
            //Console.WriteLine(xml);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            using (StreamWriter sw = new StreamWriter(new FileStream("./Alll.xml", FileMode.Create)))
            {
                sw.Write(xml);
                sw.Close();
            }
        }
        public Menu lll { get; set; }
        public static Menu GetLogConfig()
        {
            List<string> logfilelist = LogHelper.LogDataRefrush.GetLogFileList();
            List<string> date = new List<string>();
            List<string> NormalDate = new List<string>();

            for (int i = logfilelist.Count - 1; i >= 0; i--)
            {
                string tmp = logfilelist[i].Substring(0, 6);
                NormalDate.Add(tmp);
                tmp = tmp.Insert(4, "-");
                date.Add(tmp);
            }
            date = date.Distinct().ToList();
            NormalDate = NormalDate.Distinct().ToList();

            List<string> timeList = new List<string>();
            List<string> dateList = new List<string>();
            List<string> lauchTimeList = new List<string>();

            Menu lll = new Menu();
            for (int i = 0; i < date.Count; ++i)
            {
                Year year = new Year();
                year.data = date[i];
                lll.Year.Add(year);

                List<string> fileList = logfilelist.FindAll((p) => p.IndexOf(NormalDate[i]) != -1);
                for (int j = fileList.Count - 1; j >= 0; j--)
                {
                    string logText = LogHelper.LogDataRefrush.GetLogText(System.IO.Path.Combine(LogHelper.LogDataRefrush.LogFilePath, fileList[j]));

                    string pattern = @"\S\d{1,2}\/\d{1,2} \d{1,2}\:\d{2}\:\d{2}\S\s\w{2,4}\s------Logger Start------";

                    LogHelper.LogDataRefrush.GetDateAndLanuchTime(timeList, ref dateList, lauchTimeList, logText, pattern);

                    //添加二级菜单
                    string day = fileList[j].Substring(0, fileList[j].IndexOf("."));
                    day = day.Substring(6);
                    StringBuilder stringBuilder = new StringBuilder(date[i]);
                    stringBuilder.Append("-");
                    stringBuilder.Append(day);

                    Date dd = new Date() { data = stringBuilder.ToString() };
                    dd.time = new List<Time>();

                    //添加三级菜单
                    for (int l = 0; l < lauchTimeList.Count; ++l)
                    {
                        Time time = new Time();
                        time.data = lauchTimeList[l];
                        dd.time.Add(time);
                    }
                    lll.Year[i].date.Add(dd);
                }
            }
            return lll;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string xmlpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Alll.xml");

            //XmlDocument xml = new XmlDocument();
            //xml.Load(xmlpath);

            //XmlDataProvider xdp = new XmlDataProvider();
            //xdp.Document = xml;
            //xdp.XPath = @"/Menu/MenuItem";

            XmlDataProvider dd = this.FindResource("menudata") as XmlDataProvider;
            dd.Source = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Alll.xml"));
            this.item.DataContext = dd;
            //this.item.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = dd });
        }
    }
}
