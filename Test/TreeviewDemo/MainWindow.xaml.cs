using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace testTreeBinding
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InputData();
            tree.ItemsSource = getNodes(0, nodes);
        }

        private List<TreeNode> getNodes(int parentID, List<TreeNode> nodes)
        {
            List<TreeNode> mainNodes = nodes.Where(x => x.ParentID == parentID).ToList();
            List<TreeNode> otherNodes = nodes.Where(x => x.ParentID != parentID).ToList();
            foreach (TreeNode node in mainNodes)
                node.ChildNodes = getNodes(node.NodeID, otherNodes);
            return mainNodes;
        }

        private List<TreeNode> nodes;
        public static int min_third_level_nodeID = 0;
        private void InputData()
        {
            List<string> logFileList = GetLogFileList();
            List<string> date = new List<string>();
            List<string> NormalDate = new List<string>();

            for (int i = logFileList.Count - 1; i >= 0; i--)
            {
                string tmp = logFileList[i].Substring(0, 6);
                NormalDate.Add(tmp);
                tmp = tmp.Insert(4, "-");
                date.Add(tmp);

            }
            date = date.Distinct().ToList();
            NormalDate = NormalDate.Distinct().ToList();
            min_third_level_nodeID = date.Count + logFileList.Count + 1;

            List<string> timeList = new List<string>();
            List<string> dateList = new List<string>();
            List<string> lauchTimeList = new List<string>();
            nodes = new List<TreeNode>();
            int nodecount = nodes.Count;
            int seconditemID = date.Count + 1;
            int thirditemID = date.Count + logFileList.Count + 1;
            for (int i = 0; i < date.Count; ++i)
            {
                TreeNode node = new TreeNode { ParentID = 0, NodeID = i + 1, NodeName = date[i] };
                nodes.Add(node);

                List<string> fileList = logFileList.FindAll((p) => p.IndexOf(NormalDate[i]) != -1);
                for (int j = fileList.Count - 1; j >= 0; j--)
                {
                    string logText = GetLogText(System.IO.Path.Combine(@"C:\Users\29572\Desktop\llll", fileList[j]));

                    string pattern = @"\S\d{1,2}\/\d{1,2} \d{1,2}\:\d{2}\:\d{2}\S\s\w{2,4}\s------Logger Start------";

                    GetDateAndLanuchTime(timeList, ref dateList, lauchTimeList, logText, pattern);

                    //添加二级菜单
                    string day = fileList[j].Substring(0, fileList[j].IndexOf("."));
                    day = day.Substring(6);
                    StringBuilder stringBuilder = new StringBuilder(date[i]);
                    stringBuilder.Append("-");
                    stringBuilder.Append(day);

                    node = new TreeNode() { ParentID = i + 1, NodeID = seconditemID++, NodeName = stringBuilder.ToString() };
                    nodes.Add(node);

                    //添加三级菜单
                    for (int l = 0; l < lauchTimeList.Count; ++l)
                    {
                        node = new TreeNode() { ParentID = seconditemID - 1, NodeID = thirditemID++, NodeName = lauchTimeList[l].ToString() + "   BodorCut.exe" };
                        nodes.Add(node);
                    }
                }
            }
        }


        public class TreeNode
        {
            public int NodeID { get; set; }
            public int ParentID { get; set; }
            public string NodeName { get; set; }
            public List<TreeNode> ChildNodes { get; set; }
            public TreeNode()
            {
                ChildNodes = new List<TreeNode>();
            }
        }




        //////////////////////////////////////////////////////////////////////////////////////////
        public static List<string> date = new List<string>();
        public static List<string> timeList = new List<string>();
        public static List<string> dateList = new List<string>();
        public static List<string> lauchTimeList = new List<string>();
        public static List<string> logFileList = new List<string>();

        public List<string> GetLogFileList()
        {
            List<string> logFileList = new List<string>();

            try
            {
                DirectoryInfo TheFolder = new DirectoryInfo(@"C:\Users\29572\Desktop\llll");

                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (NextFile.Name.IndexOf("Normal.txt") != -1)
                    {
                        logFileList.Add(NextFile.Name);
                    }
                }
                return logFileList;
            }
            catch (Exception error)
            {
                return logFileList;
            }
        }

        private List<string> GetDateList()
        {
            for (int i = logFileList.Count - 1; i >= 0; i--)
            {
                string tmp = logFileList[i].Substring(0, 6);
                tmp = tmp.Insert(4, "-");
                date.Add(tmp);
            }
            return date;
        }

        public string GetLogText(string path)
        {
            string logText = string.Empty;

            try
            {
                FileStream fileStream = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fileStream);

                logText = sr.ReadToEnd();
                sr.Close();
                fileStream.Close();

                return logText;
            }
            catch (Exception error)
            {
                return logText;
            }
        }

        private void GetDateAndLanuchTime(List<string> timeList, ref List<string> dateList,
                                    List<string> lauchTimeList, string logText, string pattern)
        {
            timeList.Clear();
            dateList.Clear();
            lauchTimeList.Clear();

            foreach (Match match in Regex.Matches(logText, pattern))
            {
                string tmp = match.Value.Substring(match.Value.IndexOf("(") + 1, match.Value.IndexOf(")") - 1);
                timeList.Add(tmp);
            }

            foreach (var each in timeList)
            {
                dateList.Add(each.Substring(0, each.IndexOf(" ")));
                lauchTimeList.Add(each.Substring(each.IndexOf(" ") + 1));
            }
            dateList = dateList.Distinct().ToList();
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNode node = tree.SelectedItem as TreeNode;
            if (node == null)
                return;
            if (node.NodeID >= min_third_level_nodeID)
            {

                StringBuilder stringBuilder = new StringBuilder();
                string Date = nodes.Find((p) => p.NodeID == node.ParentID).NodeName;

                string normalLogDir = @"C:\Users\29572\Desktop\llll";
                string date = Date.Replace("-", "");
                stringBuilder = new StringBuilder();
                stringBuilder.Append(date);
                stringBuilder.Append(".Normal.txt");
                string normalLogFileName = stringBuilder.ToString();

                using (StreamReader sr = new StreamReader(System.IO.Path.Combine(normalLogDir, normalLogFileName), Encoding.UTF8))
                {
                    bool exit = false;
                    bool canadd = false;
                    List<string> dataList = new List<string>();
                    string Time = node.NodeName.Substring(0, node.NodeName.IndexOf(" "));
                    date = date.Substring(4);
                    string start = string.Format("({0} {1}) 普通 ------Logger Start------", date.Insert(2, "/"), Time);
                    while (!sr.EndOfStream)
                    {
                        if (exit == true)
                            break;
                        string text = sr.ReadLine();
                        if (text == start)
                            canadd = true;
                        if (text.IndexOf("Logger End") != -1 && canadd)
                            exit = true;
                        if (canadd)
                            dataList.Add(text);
                    }
                    List<Paragraph> list = GetParagraphOfLogData(dataList);
                    LogInfo.Document.Blocks.Clear();
                    for (int i = 0; i < list.Count; ++i)
                        LogInfo.Document.Blocks.Add(list[i]);
                    sr.Close();
                }
            }
        }

        public static List<Paragraph> GetParagraphOfLogData(List<string> dataList)
        {
            List<Paragraph> LogList = new List<Paragraph>();

            foreach (var each in dataList)
            {
                LogMessage logMessage = GetDetial(each);

                Paragraph paragraph = new Paragraph();
                paragraph.LineHeight = 5;

                Run run = new Run();

                if (logMessage.Time != "")
                {
                    run = new Run(logMessage.Time);
                    run.FontSize = 14;
                    run.Foreground = new SolidColorBrush(Colors.Black);
                    paragraph.Inlines.Add(run);
                }

                int Type = ConvertMessageType(logMessage.MessageType);

                switch (Type)
                {
                    case (int)LogMessageType.Normal:
                        run = new Run(logMessage.Message);
                        run.FontSize = 14;
                        run.Foreground = new SolidColorBrush(Colors.Black);
                        paragraph.Inlines.Add(run);
                        break;
                    case (int)LogMessageType.EnterModule:
                        run = new Run(logMessage.Message);
                        run.FontSize = 14;
                        run.Foreground = new SolidColorBrush(Colors.Green);
                        paragraph.Inlines.Add(run);
                        break;
                    case (int)LogMessageType.WarningInfo:
                        run = new Run(logMessage.Message);
                        run.FontSize = 14;
                        run.Foreground = new SolidColorBrush(Colors.Red);
                        paragraph.Inlines.Add(run);
                        break;
                    case (int)LogMessageType.AboutAxis:
                        run = new Run(logMessage.Message);
                        run.FontSize = 14;
                        run.Foreground = new SolidColorBrush(Colors.Aqua);
                        paragraph.Inlines.Add(run);
                        break;
                    default:
                        break;
                }
                LogList.Add(paragraph);
            }
            return LogList;
        }

        public static LogMessage GetDetial(string Line)
        {
            LogMessage logMessage = new LogMessage();
            try
            {
                if (Line.IndexOf(" ") != -1)
                {
                    var sArray = Line.Split(' ');
                    logMessage.Time = string.Format("{0} {1} ", sArray[0], sArray[1]);
                    logMessage.MessageType = sArray[2];

                    StringBuilder builder = new StringBuilder(sArray[3]);
                    for (int i = 4; i < sArray.Count(); ++i)
                    {
                        builder = builder.Append(sArray[i]);
                    }
                    logMessage.Message = builder.ToString();
                }
                else
                {
                    logMessage.MessageType = "普通";
                    logMessage.Message = Line;
                }

                return logMessage;
            }
            catch (Exception error)
            {
                //图形类的信息,类型后直接换行,不存在空格,indexof出错,直接返回
                return logMessage;
            }
        }

        public static int ConvertMessageType(string messageType)
        {
            if (messageType == "普通")
            {
                return (int)LogMessageType.Normal;
            }
            else if (messageType == "进入模块")
            {
                return (int)LogMessageType.EnterModule;
            }
            else if (messageType == "坐标相关")
            {
                return (int)LogMessageType.AboutAxis;
            }
            else
            {
                return (int)LogMessageType.WarningInfo;
            }
        }
    }

    public class LogMessage
    {
        public string Time { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
    }

    public enum LogMessageType
    {
        Normal,
        EnterModule,
        AboutAxis,
        WarningInfo
    }
}
