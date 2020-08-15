using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace testTreeBinding
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        LogHelper.LogHelper logHelper;
        public MainWindow()
        {
            InitializeComponent();
            logHelper = new LogHelper.LogHelper();
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
            List<string> logFileList = logHelper.GetLogFileList();
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
                    string logText = logHelper.GetLogText(System.IO.Path.Combine(@"C:\Users\29572\Desktop\llll", fileList[j]));

                    string pattern = @"\S\d{1,2}\/\d{1,2} \d{1,2}\:\d{2}\:\d{2}\S\s\w{2,4}\s------Logger Start------";

                    logHelper.GetDateAndLanuchTime(timeList, ref dateList, lauchTimeList, logText, pattern);

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
                    List<Paragraph> list = logHelper.GetParagraphOfLogData(dataList);
                    LogInfo.Document.Blocks.Clear();
                    for (int i = 0; i < list.Count; ++i)
                        LogInfo.Document.Blocks.Add(list[i]);
                    sr.Close();
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
    }
}
